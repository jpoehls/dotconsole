using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandLocator
    {
        ICommand GetCommandByName(string name);
        ICommand GetDefaultCommand();
        ICommandMetadata GetCommandMetadata(ICommand command);
        IDictionary<ICommand, ICommandMetadata> GetAllCommands();
    }
}