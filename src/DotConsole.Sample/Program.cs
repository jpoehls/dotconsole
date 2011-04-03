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
            var registry = new StandardRouter(new MefCommandLocator(), new StandardComposer());
        }
    }
}
