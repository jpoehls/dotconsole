using System.Collections.Generic;

namespace DotConsole
{
    public interface IRouter
    {
        ICommand GetCommand(IEnumerable<string> args);
        IEnumerable<string> FilterArguments(IEnumerable<string> args);
    }
}