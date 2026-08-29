using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator
{
    [Generator]
    public sealed class DiscriminatedUnionIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(initializationContext =>
            {
                initializationContext.AddSource(
                    "DiscriminatedUnionAttribute.g.cs",
                    @"using System;

namespace DiscriminatedUnions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DiscriminatedUnionAttribute(params Type[] caseTypes) : Attribute
{
    public Type[] CaseTypes { get; } = caseTypes;
}");
            });

            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => IsSyntaxTargetForGeneration(node),
                    transform: (syntaxContext, _) => (ClassDeclarationSyntax) syntaxContext.Node
                );

            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses, (productionContext, tuple) => GenerateDisciminatedUnionSource(productionContext, tuple.Left, tuple.Right));

            static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            {
                if (node is not ClassDeclarationSyntax classSyntax)
                {
                    return false;
                }

                const string discriminatedUnionAttributeName = "DiscriminatedUnion";

                foreach (var attributeSyntax in classSyntax.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
                {
                    var attributeName = attributeSyntax.Name.ToString();

                    if (string.Equals(discriminatedUnionAttributeName, attributeName, StringComparison.Ordinal) ||
                        string.Equals($"{discriminatedUnionAttributeName}Attribute", attributeName, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static void GenerateDisciminatedUnionSource(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classSyntaxes)
        {
            if (classSyntaxes.IsDefaultOrEmpty)
            {
                return;
            }

            var discriminatedUnionAttributeSymbol = compilation.GetTypeByMetadataName("DiscriminatedUnions.DiscriminatedUnionAttribute");
            var unionInterfaceSymbol = compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.IUnion");

            if (discriminatedUnionAttributeSymbol is null || unionInterfaceSymbol is null)
            {
                return;
            }

            foreach (var classSyntax in classSyntaxes)
            {
                // Skip not partial classes
                if (!classSyntax.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    continue;
                }

                var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);

                if (model.GetDeclaredSymbol(classSyntax, context.CancellationToken) is not  { } classSymbol)
                {
                    continue;
                }

                var discriminatedUnionAttribute = classSymbol
                    .GetAttributes()
                    .FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, discriminatedUnionAttributeSymbol));

                if (discriminatedUnionAttribute is null || discriminatedUnionAttribute.ConstructorArguments.Length != 1)
                {
                    continue;
                }

                var caseTypes = discriminatedUnionAttribute.ConstructorArguments[0].Values
                    .Select(argument => argument.Value as ITypeSymbol)
                    .Where(caseType => caseType is not null && caseType.TypeKind != TypeKind.Error)
                    .ToList();

                if (caseTypes.Count == 0)
                {
                    continue;
                }

                var cases = BuildCases(caseTypes!);

                var className = classSymbol.Name;
                var accessibility = classSymbol.DeclaredAccessibility == Accessibility.Public ? "public" : "internal";

                var namespaceDeclaration = classSymbol.ContainingNamespace.IsGlobalNamespace
                    ? string.Empty
                    : $"namespace {classSymbol.ContainingNamespace.ToDisplayString()};\n\n";

                var source = GenerateClass(namespaceDeclaration, accessibility, className, cases);

                context.AddSource($"{className}.DiscriminatedUnion.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static List<(string Type, string Name)> BuildCases(List<ITypeSymbol> caseTypes)
        {
            var cases = new List<(string Type, string Name)>(caseTypes.Count);

            foreach (var caseType in caseTypes)
            {
                var name = caseType.Name.Length > 0 ? caseType.Name : "Case";
                var candidate = name;
                var suffix = 1;

                while (cases.Any(existing => string.Equals(existing.Name, candidate, StringComparison.Ordinal)))
                {
                    candidate = name + ++suffix;
                }

                cases.Add((caseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), candidate));
            }

            return cases;
        }


        private static string GenerateClass(string namespaceDeclaration, string accessibility, string className, List<(string Type, string Name)> cases)
        {
            var constructors = string.Join("\n\n", cases.Select(unionCase => $@"    public {className}({unionCase.Type} value)
        : this((object?) value, Discriminator.{unionCase.Name})
    {{
    }}"));

            var discriminatorMembers = string.Join("\n", cases.Select((unionCase, index) => $"        {unionCase.Name} = {index},"));

            var readCases = string.Join("\n", cases.Select(unionCase => $@"                case ""{unionCase.Name}"":
                    return new {className}(root.Deserialize<{unionCase.Type}>(options)!);"));

            var writeCases = string.Join("\n", cases.Select(unionCase => $@"                case {className}.Discriminator.{unionCase.Name}:
                    writer.WriteString(Discriminator, ""{unionCase.Name}"");
                    WriteValue(writer, JsonSerializer.SerializeToElement<{unionCase.Type}>(({unionCase.Type}) value.Value!, options));
                    break;"));

            return $@"
#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

{namespaceDeclaration}[Union]
[JsonConverter(typeof({className}.DiscriminatedUnionJsonConverter))]
{accessibility} partial class {className} : IUnion
{{
    private readonly object? _value;
    private readonly Discriminator _discriminator;

    private {className}(object? value, Discriminator discriminator)
    {{
        _value = value;
        _discriminator = discriminator;
    }}

{constructors}

    public object? Value => _value;

    private enum Discriminator
    {{
{discriminatorMembers}
    }}

    private sealed class DiscriminatedUnionJsonConverter : JsonConverter<{className}>
    {{
        private const string Discriminator = ""$discriminator"";

        public override {className}? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {{
            using var document = JsonDocument.ParseValue(ref reader);

            var root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
            {{
                throw new JsonException($""Expected a JSON object for '{className}' but found {{root.ValueKind}}."");
            }}

            if (!root.TryGetProperty(Discriminator, out var discriminatorElement) || discriminatorElement.ValueKind != JsonValueKind.String)
            {{
                throw new JsonException($""Missing '{{Discriminator}}' for '{className}'."");
            }}

            var discriminator = discriminatorElement.GetString();

            switch (discriminator)
            {{
{readCases}
                default:
                    throw new JsonException($""Unknown '{{Discriminator}}' value '{{discriminator}}' for '{className}'."");
            }}
        }}

        public override void Write(Utf8JsonWriter writer, {className} value, JsonSerializerOptions options)
        {{
            writer.WriteStartObject();

            switch (value._discriminator)
            {{
{writeCases}
                default:
                    throw new JsonException($""Unknown discriminator '{{value._discriminator}}' on '{className}'."");
            }}

            writer.WriteEndObject();
        }}

        private static void WriteValue(Utf8JsonWriter writer, JsonElement element)
        {{
            if (element.ValueKind != JsonValueKind.Object)
            {{
                throw new JsonException($""A case of '{className}' must serialize to a JSON object but produced {{element.ValueKind}}."");
            }}

            foreach (var property in element.EnumerateObject())
            {{
                property.WriteTo(writer);
            }}
        }}
    }}
}}
";
        }
    }
}
