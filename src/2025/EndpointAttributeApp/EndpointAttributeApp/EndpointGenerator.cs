using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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
            .Where(classSyntax => classSyntax is not null)
            .Select((classSyntax, _) => classSyntax!);

        var classSymbols = classDeclarations
            .Select((classSyntax, cancellationToken) =>
            {
                var model = context.SemanticModel;
                var classSymbol = model.GetDeclaredSymbol(classSyntax) as INamedTypeSymbol;

                return classSymbol;
            })
            .Where(static s => s is not null);

        context.RegisterSourceOutput(classSymbols, GenerateEndpointSource);

        static bool IsEndpointSyntaxNode(SyntaxNode node) =>
            node is ClassDeclarationSyntax syntax &&
            syntax.AttributeLists.Any();

        static ClassDeclarationSyntax? ToClassDeclarationSyntax(GeneratorSyntaxContext syntaxContext)
        {
            
        }
    }

    private void GenerateEndpointSource(SourceProductionContext context, INamedTypeSymbol classSymbol)
    {
        var endpointAttrSymbol = context.Compilation.GetTypeByMetadataName("EndpointAttribute");
        var handlerInterfaceSymbol = context.Compilation.GetTypeByMetadataName("IHandler`2");

        if (endpointAttrSymbol is null || handlerInterfaceSymbol is null)
            return;

        var endpointAttr = classSymbol.GetAttributes().FirstOrDefault(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, endpointAttrSymbol));

        if (endpointAttr is null)
            return;

        var handlerInterface = classSymbol.AllInterfaces.FirstOrDefault(i =>
            i.OriginalDefinition.Equals(handlerInterfaceSymbol, SymbolEqualityComparer.Default));

        if (handlerInterface is null)
            return;

        var verb = endpointAttr.ConstructorArguments[0].Value?.ToString();
        var route = endpointAttr.ConstructorArguments[1].Value?.ToString();

        var tCommand = handlerInterface.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var tResult = handlerInterface.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var source = GenerateClass(classSymbol.Name, verb, route, tCommand, tResult);

        context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
    }
    
    private string GenerateClass(string className, string? verb, string? route, string tCommand, string tResult)
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