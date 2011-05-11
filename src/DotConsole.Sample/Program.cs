using System;

namespace DotConsole.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commander = new Commander();
            commander.Run();
            Console.ReadLine();
        }
    }
}
