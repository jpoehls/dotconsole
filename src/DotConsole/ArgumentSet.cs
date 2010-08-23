using System;
using System.Collections.Generic;

namespace DotConsole
{
    public class ArgumentSet
    {
        public ArgumentSet()
        {
            NamedArguments = new Dictionary<string, IArgumentValue>(StringComparer.OrdinalIgnoreCase);
            PositionalArguments = new List<string>();
        }

        public Dictionary<string, IArgumentValue> NamedArguments { get; private set; }
        public List<string> PositionalArguments { get; private set; }
    }

    public class ArgumentValueFactory
    {
        
    }
}