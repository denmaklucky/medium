using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SealedClassAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SealedClassRule : DiagnosticAnalyzer
{
    internal const string RuleId = "RULE0001";
    
    private static readonly DiagnosticDescriptor Rule = new(
        RuleId,
        title: "The class have to be sealed",
        messageFormat: "Class can be sealed",
        category: "Unknown",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeSealedClass, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeSealedClass(SyntaxNodeAnalysisContext context)
    {
        var classSyntax = (ClassDeclarationSyntax)context.Node;

        var skipRuleGeneration = classSyntax.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.AbstractKeyword) ||
                                                                       modifier.IsKind(SyntaxKind.SealedKeyword));

        if (skipRuleGeneration)
        {
            return;
        }

        var diagnostic = Diagnostic.Create(Rule, classSyntax.Keyword.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }
}