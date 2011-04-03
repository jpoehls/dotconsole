namespace DotConsole
{
    public interface ICommandLocator
    {
        ICommand GetCommand(string name);
    }
}