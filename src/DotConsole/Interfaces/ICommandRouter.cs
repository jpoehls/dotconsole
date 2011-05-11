using System.Collections.Generic;

namespace DotConsole
{
    internal interface ICommandRouter
    {
        ICommand Route(IEnumerable<string> args);
        string GetCommandName(IEnumerable<string> args);
    }
}