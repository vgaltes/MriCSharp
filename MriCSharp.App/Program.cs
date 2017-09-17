using System;

namespace MriCSharp.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var operation = args[0];

            switch(operation)
            {
                case "n":
                case "methodNames":
                    var methodNames = MethodDetector.GetMethodNamesOf(args[1]);
                    foreach(var methodName in methodNames)
                    {
                        Console.WriteLine(methodName);
                    }
                break;
                case "d":
                case "diff":
                    var diffResult = DiffCalculator.GetDiffBetween(args[1], args[2], args[3]);
                    Console.WriteLine(diffResult.HasChanged);
                    Console.WriteLine(diffResult.Churn);
                break;
            }
            
        }
    }
}
