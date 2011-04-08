using System;

namespace DotConsole.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commander = Commander.Standard();
            commander.Run();
            Console.ReadLine();
        }
    }
}
