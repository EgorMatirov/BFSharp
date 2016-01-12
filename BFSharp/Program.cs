using System;
using System.Linq;

namespace BFSharp
{
    internal class Program
    {
        private static string CodeForPrinting(char ch)
        {
            return new string('+', ch) + ".";
        }

        private static void Main()
        {
            const string toBePrinted = "Hello world!\n";
            var program = string.Concat(toBePrinted.SelectMany(ch => CodeForPrinting(ch) + '>'));
            Console.WriteLine(program);
            var runner = new BrainfuckRunner(new ArrayMemory());
            runner.Run(program, Console.Read, Console.Write);
        }
    }
}
