#! "netcoreapp2.1"
#r "nuget:Microsoft.CodeAnalysis.CSharp,2.8.2"

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var tree = CSharpSyntaxTree.ParseText(@"
using System;
using System.Collection;
using System.Linq;
using System.Text;

namespace HelloWorld {
    class Program {
        static void Main(String[] args) {
            Console.WriteLine(""Hello, world!"");
        }
    }
}
");

void GetDelcaration() {
    var root = tree.GetRoot() as CompilationUnitSyntax;
    var firstMember = root.Members[0];
    var helloWorldDeclration = firstMember as NamespaceDeclarationSyntax;
    var programDeclaration = helloWorldDeclration.Members[0] as ClassDeclarationSyntax;
    var mainDeclaration = programDeclaration.Members[0] as MethodDeclarationSyntax;
    var argsParameter = mainDeclaration.ParameterList.Parameters[0];

    Console.WriteLine(mainDeclaration);
    Console.WriteLine(argsParameter);
}

void QueryArgs() {
    var root = tree.GetRoot() as CompilationUnitSyntax;
    var firstParameters =
        from methods in root.DescendantNodes().OfType<MethodDeclarationSyntax>()
        where methods.Identifier.ValueText == "Main"
        select methods.ParameterList.Parameters.First();

    var argsParameter2 = firstParameters.Single();
    Console.WriteLine(argsParameter2);
}

