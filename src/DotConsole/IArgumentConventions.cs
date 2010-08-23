using System;
using System.Linq;

namespace DotConsole
{
    public interface IArgumentConventions
    {
        bool IsNamed(string arg);
        string GetName(string arg);
        IArgumentValue GetValue(string arg);
    }
}