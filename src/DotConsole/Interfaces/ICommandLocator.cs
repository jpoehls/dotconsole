using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandLocator
    {
        void RegisterCommand<TCommand>() where TCommand : ICommand;
        ICommand GetCommandByName(string name);
        ICommandMetadata GetCommandMetadata(ICommand command);
        IDictionary<ICommand, ICommandMetadata> GetAllCommands();
    }
}