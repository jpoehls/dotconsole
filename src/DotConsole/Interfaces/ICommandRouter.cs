using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandRouter
    {
        ICommandLocator Locator { get; }
        ICommand Route(IEnumerable<string> args);
    }
}