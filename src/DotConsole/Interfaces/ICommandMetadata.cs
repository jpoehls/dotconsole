namespace DotConsole
{
    public interface ICommandMetadata
    {
        string Name { get; }
        bool IsDefault { get; }
    }
}