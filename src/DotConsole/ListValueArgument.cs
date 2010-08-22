using System.Collections.Generic;

namespace DotConsole
{
    public class ListValueArgument : IArgumentValue
    {
        public ListValueArgument()
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