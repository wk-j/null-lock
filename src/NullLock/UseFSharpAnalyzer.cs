using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NullLock {

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseFSharpAnalyzer : DiagnosticAnalyzer {

        static DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                "UseFSharpAnalyzer",
                "Use F#",
                "You are using C# use F# insead",
                "Lanuage Choice",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                helpLinkUri: "http://fsharp.org"
            );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) {
            context.RegisterSyntaxTreeAction(AnalyzeTree);
        }

        private static void AnalyzeTree(SyntaxTreeAnalysisContext context) {
            var rootLocation = context.Tree.GetRoot().GetLocation();
            var diag = Diagnostic.Create(Rule, rootLocation);
            context.ReportDiagnostic(diag);
        }
    }
}