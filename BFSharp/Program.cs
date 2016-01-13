using System;

namespace BFSharp
{
    internal class Program
    {
        private static void Main()
        {
            const string program = ">+[>,]<[<]>>[.>]";
            const string input = "Hello\n\0";
            var i = 0;
            Func<int> readFunc = () => input[i++];
            Console.WriteLine(program);
            var runner = new BrainfuckRunner(new ArrayMemory(), program, readFunc, Console.Write);
            runner.Run();
        }
    }
}