namespace DotConsole.Tests.Commands
{
    [Command("help", "?", IsDefault = true)]
    public class HelpCommand : ICommand
    {
        [Parameter(Position = 0)]
        public string TopicName { get; set; }

        [Parameter(Position = 1)]
        public string Verbosity { get; set; }

        public void Execute()
        { }
    }
}