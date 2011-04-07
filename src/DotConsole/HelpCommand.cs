namespace DotConsole
{
    [Command("help", "?", IsDefault = true)]
    public class HelpCommand : ICommand
    {
        [Parameter("command", Position = 0)]
        public string CommandName { get; set; }

        public void Execute()
        {
            // todo: generate help output here
        }
    }
}