using System;
using System.Collections.Generic;

namespace DotConsole
{
    public class ArgumentSet
    {
        public ArgumentSet()
        {
            Flags = new List<char>();
            Arguments = new Dictionary<string, IArgumentValue>(StringComparer.OrdinalIgnoreCase);
            PositionalArguments = new List<string>();
        }

        public List<char> Flags { get; private set; }
        public Dictionary<string, IArgumentValue> Arguments { get; private set; }
        public List<string> PositionalArguments { get; private set; }
    }
}