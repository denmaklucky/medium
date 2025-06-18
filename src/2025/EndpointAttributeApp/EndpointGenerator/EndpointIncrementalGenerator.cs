using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EndpointGenerator
{
    [Generator]
    public sealed class EndpointIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(initializationContext =>
            {
                initializationContext.AddSource(
                    "EndpointAttribute.g.cs",
                    @"using System;

namespace EndpointGenerator;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EndpointAttribute(string httpVerb, string route) : Attribute
{
    public string HttpVerb { get; } = httpVerb;

    public string Route { get; } = route;
}");

                initializationContext.AddSource(
                    "IHandler.g.cs",
                    @"namespace EndpointGenerator;

public interface IHandler<in TCommand, TResult>
{
    Task<TResult> InvokeAsync(TCommand command);
}");

                initializationContext.AddSource(
                    "IEndpoint.g.cs",
                    @"namespace EndpointGenerator;

public interface IEndpoint
{
    void Configure(IEndpointRouteBuilder app);
}");
            });

            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => IsSyntaxTargetForGeneration(node),
                    transform: (syntaxContext, _) => (ClassDeclarationSyntax) syntaxContext.Node
                );

            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses, (productionContext, tuple) => GenerateEndpointSource(productionContext, tuple.Left, tuple.Right));

            static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            {
                if (node is not ClassDeclarationSyntax classSyntax)
                {
                    return false;
                }
            
                const string endpointAttributeName = "Endpoint";
            
                foreach (var attributeSyntax in classSyntax.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
                {
                    var attributeName = attributeSyntax.Name.ToString();
            
                    if (string.Equals(endpointAttributeName, attributeName, StringComparison.Ordinal) || 
                        string.Equals($"{endpointAttributeName}Attribute", attributeName, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            
                return false;
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
        
                if (model.GetDeclaredSymbol(classSyntax, context.CancellationToken) is not INamedTypeSymbol classSymbol)
                {
                    continue;
                }
        
                var endpointAttributeSymbol = compilation.GetTypeByMetadataName("EndpointGenerator.EndpointAttribute");
                var handlerSymbol = compilation.GetTypeByMetadataName("EndpointGenerator.IHandler`2");

                if (endpointAttributeSymbol is null || handlerSymbol is null)
                {
                    return;
                }
            
                var endpointAttribute = classSymbol.GetAttributes()
                    .FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, endpointAttributeSymbol));

                if (endpointAttribute is null)
                {
                    return;
                }
            
                var handler = classSymbol.AllInterfaces
                    .FirstOrDefault(i => i.OriginalDefinition.Equals(handlerSymbol, SymbolEqualityComparer.Default));

                if (handler is null)
                {
                    return;
                }
            
                var verb = endpointAttribute.ConstructorArguments[0].Value?.ToString();
                var route = endpointAttribute.ConstructorArguments[1].Value?.ToString();
            
                var command = handler.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            
                var handlerName = classSymbol.Name;
                var clientNamespace = classSymbol.ContainingNamespace.ToDisplayString();

                var className = handlerName.Replace("Handler", "Endpoint");
                
                var source = GenerateClass(clientNamespace, className, handlerName, verb, route, command);
        
                context.AddSource($"{className}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
    
        private static string GenerateClass(string @namespace, string className, string handlerName, string? verb, string? route, string commandType)
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

            return $@"using EndpointGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace {@namespace};

public sealed class {className} : IEndpoint
{{
    public void Configure(IEndpointRouteBuilder app)
    {{
        app.{mapMethod}(""{route}"", InvokeAsync);
    }}

    private static async Task<IResult> InvokeAsync([FromServices] {handlerName} handler, {commandType} command)
    {{
        var result = await handler.InvokeAsync(command);

        return TypedResults.Ok(result);
    }}
}}
";
        }
    }
}