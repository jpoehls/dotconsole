namespace DotConsole
{
    public interface ICommandLocator
    {
        ICommand GetCommand(string name);
        ICommand GetDefaultCommand();
        ICommandMetadata GetCommandMetadata(ICommand command);
    }
}