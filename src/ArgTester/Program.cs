using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgTester
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine();
            Console.WriteLine("Printing out the [Environment]::GetCommandLineArgs() array...");
            Console.WriteLine("(Spaces are shown as a · for visibility.)");
            Console.WriteLine();

            var args = Environment.GetCommandLineArgs();
            var count = args.Length;
            var prefixWidth = count.ToString().Length + 4;
            
            for (int i = 0; i < count; i++)
            {
                Console.Write(string.Format("[{0}]: ", i).PadRight(prefixWidth, ' '));
                Console.WriteLine(args[i].Replace(' ', '·'));
            }

            Console.WriteLine();
        }
    }
}
