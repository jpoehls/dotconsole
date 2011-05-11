using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotConsole.Tests.StubCommands
{
    [Command("testmigrate")]
    public class TestMigrateCommand : ICommand
    {
        [Required(ErrorMessage = "Connection is required.")]
        [Parameter("connection", Flag = 'c', Position = 0)]
        public string Connection { get; set; }

        [Parameter("version", Flag = 'v', Position = 1)]
        public long TargetVersion { get; set; }

        public void Execute()
        { }
    }
}