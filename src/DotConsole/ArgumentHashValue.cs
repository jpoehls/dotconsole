using System.Collections.Generic;

namespace DotConsole
{
    public class ArgumentHashValue : IArgumentValue
    {
        public ArgumentHashValue()
        {
            Values = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Values { get; private set; }

        public void SetValue(string arg)
        {
            var split = arg.Split('=');
            Values.Add(split[0], split[1]);
        }
    }
}