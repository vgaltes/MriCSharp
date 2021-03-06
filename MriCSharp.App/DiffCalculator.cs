using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MriCSharp.App
{
    public static class DiffCalculator
    {
        public static DiffResult GetDiffBetween(string fileBefore, string fileAfter, string methodName)
        {
            var fileBeforeContents = File.Exists(fileBefore) ? File.ReadAllText(fileBefore) : string.Empty;
            var fileAfterContents = File.Exists(fileAfter) ? File.ReadAllText(fileAfter) : string.Empty;

            var codeBefore = GetMethodBody(fileBeforeContents, methodName);
            var codeAfter = GetMethodBody(fileAfterContents, methodName);

            var (haveChangedBefore, churnBefore, linesSharedBefore) = GetDiffDataBetween(codeBefore, codeAfter, (c) => c += 1);

            var (haveChangedAfter, churnAfter, linesSharedAfter) = GetDiffDataBetween(codeAfter, codeBefore, (c) => c += 1);

            var (haveChangedSharedLines, churnSharedLines) = GetDiffBetweenSharedLines(linesSharedBefore, linesSharedAfter);

            return new DiffResult(haveChangedSharedLines || haveChangedAfter || haveChangedBefore, churnSharedLines + churnAfter + churnBefore);
        }

        private static Tuple<bool, int, Dictionary<string, int>> GetDiffDataBetween(IEnumerable<string> codeFrom, IEnumerable<string> codeTo, Func<int, int> churnOperation)
        {
            var haveChanged = false;
            var churn = 0;
            var linesShared = new Dictionary<string, int>();

            foreach (var lineFileFrom in codeFrom)
            {
                if (!codeTo.Contains(lineFileFrom))
                {
                    haveChanged = true;
                    churn = churnOperation(churn);
                }
                else
                {
                    AddToLinesShared(linesShared, lineFileFrom);
                }
            }

            return Tuple.Create(haveChanged, churn, linesShared);
        }

        private static void AddToLinesShared(Dictionary<string, int> linesSharedBefore, string lineFileBefore)
        {
            if (!linesSharedBefore.ContainsKey(lineFileBefore))
            {
                linesSharedBefore.Add(lineFileBefore, 1);
            }
            else
            {
                linesSharedBefore[lineFileBefore]++;
            }
        }

        private static Tuple<bool, int> GetDiffBetweenSharedLines(IDictionary<string, int> sharedLinesFrom, IDictionary<string, int> sharedLinesTo)
        {
            var haveChanged = false;
            var churn = 0;

            foreach (var line in sharedLinesFrom.Keys)
            {
                var localChurn = (sharedLinesTo[line] - sharedLinesFrom[line]);
                if (localChurn != 0)
                {
                    churn += localChurn;
                    haveChanged = true;
                }
            }

            return Tuple.Create(haveChanged, churn);
        }

        private static IEnumerable<string> GetMethodBody(string code, string methodName)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var methods = root.DescendantNodes()
                                 .OfType<MethodDeclarationSyntax>()
                                 .Where(n => n.Identifier.ValueText == methodName);

            if (methods.Count() > 0)
            {
                var allTogether = string.Join("\n", methods.Select(m => m.ToString()));                
                return allTogether.Split('\n');
            }
            
            return Enumerable.Empty<string>();
        }
    }

    public struct DiffResult
    {
        public bool HasChanged{ get;}
        public int Churn{ get;}

        public DiffResult(bool hasChanged, int churn)
        {
            HasChanged = hasChanged;
            Churn = churn;
        }
    }
}