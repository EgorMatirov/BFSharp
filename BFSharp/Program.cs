using System;

namespace BFSharp
{
    internal class Program
    {
        private static void Main()
        {
            const string program =
                ">++++++++[-<+++++++++>]<.>>+>-[+]++>++>+++[>[->+++<<+++>]<<]>-----.>->+++..+++.>-.<<+[>[+>+]>>]<--------------.>>.+++.------.--------.>+.>+.";
            Console.WriteLine(program);
            var runner = new BrainfuckRunner(new ArrayMemory());
            runner.Run(program, Console.Read, Console.Write);
        }
    }
}