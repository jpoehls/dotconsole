namespace DotConsole.Tests.StubCommands
{
    [Command("testhelp")]
    public class TestHelpCommand : ICommand
    {
        [Parameter(0)]
        public string TopicName { get; set; }

        [Parameter(1)]
        public string Verbosity { get; set; }

        public void Execute()
        { }
    }
}