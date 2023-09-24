using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SealedClassAnalyzer;

[Shared]
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SealedClassCodeFix))]
public sealed class SealedClassCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SealedClassRule.RuleId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        var classDeclaration = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

        if (classDeclaration == null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Make the class sealed",
                createChangedDocument: _ => MakeClassSealedAsync(root!, context.Document, classDeclaration),
                equivalenceKey: nameof(SealedClassCodeFix)),
            diagnostic);
    }

    private static Task<Document> MakeClassSealedAsync(SyntaxNode root, Document document, ClassDeclarationSyntax @class)
    {
        var newClass = @class.WithModifiers(@class.Modifiers.Add(SyntaxFactory.Token(SyntaxKind.SealedKeyword)));

        var newRoot = root.ReplaceNode(@class, newClass);

        return Task.FromResult(document.WithSyntaxRoot(newRoot));
    }
}