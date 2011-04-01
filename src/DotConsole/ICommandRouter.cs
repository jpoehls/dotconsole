using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandRouter
    {
        ICommand Route();
        ICommand Route(IEnumerable<string> args);
    }
}