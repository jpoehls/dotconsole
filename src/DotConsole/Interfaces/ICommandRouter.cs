using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandRouter
    {
        ICommand Route(IEnumerable<string> args);
        string GetCommandName(IEnumerable<string> args);
    }
}