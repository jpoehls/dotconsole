using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotConsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var commander = Commander.Standard();
            commander.Run();
            Console.ReadLine();
        }
    }
}
