using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointAttributeApp;

[Generator]
public sealed class EndpointGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                (node, _) => IsEndpointSyntaxNode(node),
                (syntaxContext, _) => ToClassDeclarationSyntax(syntaxContext)
            )
            .Where(classSyntax => classSyntax != null)
            .Select((classSyntax, _) => classSyntax!);

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, (productionContext, tuple) => GenerateEndpointSource(productionContext, tuple.Left, tuple.Right));

        static bool IsEndpointSyntaxNode(SyntaxNode node) =>
            node is ClassDeclarationSyntax syntax &&
            syntax.AttributeLists.Any();

        static ClassDeclarationSyntax? ToClassDeclarationSyntax(GeneratorSyntaxContext syntaxContext)
        {
            if (syntaxContext.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return null;
            }

            const string endpointAttributeName = "Endpoint";
            
            foreach (var attributeSyntax in classDeclarationSyntax.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
            {
                var attributeName = attributeSyntax.Name.ToString();

                if (string.Equals(endpointAttributeName, attributeName, StringComparison.Ordinal) || 
                    string.Equals($"{endpointAttributeName}Attribute", attributeName, StringComparison.Ordinal))
                {
                    return classDeclarationSyntax;
                }
            }

            return null;
        }
    }

    private static void GenerateEndpointSource(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classSyntaxes)
    {
        if (classSyntaxes.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var classSyntax in classSyntaxes)
        {
            var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);
        }

        // var endpointAttrSymbol = context.Compilation.GetTypeByMetadataName("EndpointAttribute");
        // var handlerInterfaceSymbol = context.Compilation.GetTypeByMetadataName("IHandler`2");
        //
        // if (endpointAttrSymbol is null || handlerInterfaceSymbol is null)
        //     return;
        //
        // var endpointAttr = classSymbol.GetAttributes().FirstOrDefault(attr =>
        //     SymbolEqualityComparer.Default.Equals(attr.AttributeClass, endpointAttrSymbol));
        //
        // if (endpointAttr is null)
        //     return;
        //
        // var handlerInterface = classSymbol.AllInterfaces.FirstOrDefault(i =>
        //     i.OriginalDefinition.Equals(handlerInterfaceSymbol, SymbolEqualityComparer.Default));
        //
        // if (handlerInterface is null)
        //     return;
        //
        // var verb = endpointAttr.ConstructorArguments[0].Value?.ToString();
        // var route = endpointAttr.ConstructorArguments[1].Value?.ToString();
        //
        // var tCommand = handlerInterface.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        // var tResult = handlerInterface.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        //
        // var source = GenerateClass(classSymbol.Name, verb, route, tCommand, tResult);
        //
        // context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
    }
    
    private static string GenerateClass(string className, string? verb, string? route, string tCommand, string tResult)
    {
        var mapMethod = verb?.ToLowerInvariant() switch
        {
            "get" => "MapGet",
            "post" => "MapPost",
            "put" => "MapPut",
            "delete" => "MapDelete",
            "patch" => "MapPatch",
            _ => "MapGet"
        };

        return $@"
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.HttpResults;

public partial class {className} : IEndpoint
{{
    public void Configure(IEndpointRouteBuilder app)
    {{
        app.{mapMethod}(""{route}"", InvokeAsync);
    }}

    private static async Task<IResult> InvokeAsync({className} handler, {tCommand} command)
    {{
        var result = await handler.InvokeAsync(command);

        return TypedResults.Ok(result);
    }}
}}
";
    }
}