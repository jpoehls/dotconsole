using System;
using System.Linq;

namespace DotConsole
{
    public interface IRouter
    {
        ICommand GetCommand(ArgumentSet args);
    }
}