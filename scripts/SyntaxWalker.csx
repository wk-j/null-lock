#! "netcoreapp2.1"
#r "nuget:Microsoft.CodeAnalysis.CSharp,2.8.2"

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var tree = CSharpSyntaxTree.ParseText(@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TopLevel
{
    using Microsoft;
    using System.ComponentModel;

    namespace Child1
    {
        using Microsoft.Win32;
        using System.Runtime.InteropServices;

        class Foo { }
    }

    namespace Child2
    {
        using System.CodeDom;
        using Microsoft.CSharp;

        class Bar { }
    }
}"
);


public class UsingCollector : CSharpSyntaxWalker {
    public readonly List<UsingDirectiveSyntax> Usings = new List<UsingDirectiveSyntax>();

    public override void VisitUsingDirective(UsingDirectiveSyntax node) {
        if (node.Name.ToString() != "System" &&
            !node.Name.ToString().StartsWith("System.")
        ) {
            this.Usings.Add(node);
        }
    }
}

var root = tree.GetRoot() as CompilationUnitSyntax;
var collector = new UsingCollector();
collector.Visit(root);

foreach (var us in collector.Usings) {
    Console.WriteLine(us);
}
