using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Buildalyzer;
using Buildalyzer.Workspaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Threading;
using System.Collections.Generic;

namespace NullLock {

    class Program {
        static async Task Main(string[] args) {
            var project = LoadProject("/Users/wk/Source/NullLock/tests/MyApp/MyApp.csproj");

            var analyzers = GetAllAnalyzers();
            foreach (var item in analyzers) {
                Console.WriteLine("~ {0}", item.ToString());
            }

            var diagonostics = await GetProjectAnalyzerDiagnosticsAsync(project, analyzers);

            foreach (var item in diagonostics) {
                Console.WriteLine("! {0}", item);
            }
        }

        static Project LoadProject(string path) {
            var manager = new AnalyzerManager();
            var analizer = manager.GetProject(path);
            var workspace = new AdhocWorkspace();
            var roslyn = analizer.AddToWorkspace(workspace, true);
            return roslyn;
        }

        static ImmutableArray<DiagnosticAnalyzer> GetAllAnalyzers() {
            var builder = ImmutableArray.CreateBuilder<DiagnosticAnalyzer>();
            var list = new List<DiagnosticAnalyzer> {
                new UseFSharpAnalyzer()
            };
            builder.AddRange(list);
            return builder.ToImmutable();
        }

        static async Task<ImmutableArray<Diagnostic>> GetProjectAnalyzerDiagnosticsAsync(Project project, ImmutableArray<DiagnosticAnalyzer> analzyers) {
            var cancel = new CancellationTokenSource();
            var supportedDiagnosticsSpecificOptions = new Dictionary<string, ReportDiagnostic>();
            analzyers.Select(x => x.SupportedDiagnostics).SelectMany(x => x).ToList().ForEach(x => {
                supportedDiagnosticsSpecificOptions[x.Id] = ReportDiagnostic.Default;
            });

            supportedDiagnosticsSpecificOptions.Add("AD001", ReportDiagnostic.Error);
            var modifiedSpecificDiagnosticOptions = supportedDiagnosticsSpecificOptions.ToImmutableDictionary().SetItems(project.CompilationOptions.SpecificDiagnosticOptions);
            var modifiedCompilationOptions = project.CompilationOptions.WithSpecificDiagnosticOptions(modifiedSpecificDiagnosticOptions);
            var compilation = await project.GetCompilationAsync(cancel.Token).ConfigureAwait(false);
            var compalationWithAnalyzers = compilation.WithAnalyzers(analzyers, cancellationToken: cancel.Token);
            var diagnostics = await compalationWithAnalyzers.GetAllDiagnosticsAsync();

            var result = compalationWithAnalyzers.GetAnalysisResultAsync(cancel.Token);
            return diagnostics;
        }
    }
}