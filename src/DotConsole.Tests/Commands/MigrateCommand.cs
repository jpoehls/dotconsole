namespace DotConsole.Tests.Commands
{
    [Command("migrate")]
    public class MigrateCommand : ICommand
    {
        [Parameter("connection", "c", Position = 0)]
        public string Connection { get; set; }

        [Parameter("version", "v", Position = 1)]
        public long TargetVersion { get; set; }

        public void Execute()
        { }
    }
}