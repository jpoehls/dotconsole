using System;
using System.Linq;

namespace DotConsole
{
    public interface IArgumentParser
    {
        ArgumentSet Parse(string[] args);
    }
}