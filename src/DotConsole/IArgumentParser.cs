using System.Collections.Generic;

namespace DotConsole
{
    public interface IArgumentParser
    {
        ArgumentSet Parse(string[] args);
    }
}