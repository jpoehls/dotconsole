using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandLocator
    {
        ICommand GetCommandByName(string name);
        ICommandMetadata GetCommandMetadata(ICommand command);
        IDictionary<ICommand, ICommandMetadata> GetAllCommands();
    }
}