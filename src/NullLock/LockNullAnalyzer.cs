using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;

namespace NullLock {
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LockNullAnalyzer : DiagnosticAnalyzer {

        static DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                "P911",
                "Lock with null expression",
                "'<null>' is not a reference type as required by the lock statement.",
                "Null",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                helpLinkUri: "http://fsharp.org"
            );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) {
            context.RegisterSyntaxTreeAction(AnalyzeTree);
        }

        private static void AnalyzeTree(SyntaxTreeAnalysisContext context) {
            var root = context.Tree.GetRoot();
            var nodes = root.DescendantNodes().OfType<LockStatementSyntax>();

            foreach (var item in nodes) {
                var expression = item.Expression;
                var kind = expression.Kind();
                if (kind == SyntaxKind.NullLiteralExpression) {
                    var dig = Diagnostic.Create(Rule, expression.GetLocation());
                    context.ReportDiagnostic(dig);
                }
            }
        }
    }
}