#! "netcoreapp2.1"
#r "nuget:Buildalyzer,1.0.0"
#r "nuget:Buildalyzer.Workspaces,1.0.0"

using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;

var manager = new AnalyzerManager();
var analyzer = manager.GetProject("tests/MyApp/MyApp.csproj");

var rs = analyzer.GetWorkspace();

// var workspace = new AdhocWorkspace();
// analyzer.AddToWorkspace(workspace); // Process is terminating due to StackOverflowException.