using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MriCSharp.App
{
    public static class MethodDetector
    {
        public static IEnumerable<string> GetMethodNamesOf(string filePath)
        {
            var code = System.IO.File.ReadAllText(filePath);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            return root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Select(m => m.Identifier.ValueText);
        }
    }
}