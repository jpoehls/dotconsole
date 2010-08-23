using System.Collections.Generic;

namespace DotConsole
{
    public class ArgumentListValue : IArgumentValue
    {
        public ArgumentListValue()
        {
            Values = new List<string>();
        }

        public List<string> Values { get; private set; }

        public void SetValue(string arg)
        {
            Values.Add(arg);
        }
    }
}