using System;

namespace DotConsole
{
    public class ArgumentFlagValue : IArgumentValue
    {
        public void SetValue(string arg)
        {
            throw new NotSupportedException();
        }
    }
}