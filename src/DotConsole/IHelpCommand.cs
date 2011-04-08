using System.Collections.Generic;

namespace DotConsole
{
    public interface IHelpCommand : ICommand
    {
        ICommandLocator CommandLocator { get; set; }
        IEnumerable<string> ErrorMessages { get; set; }
    }
}