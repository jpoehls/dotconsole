using System.Collections.Generic;

namespace DotConsole.Tests.Commands
{
    public class MigrateCommand : ICommand
    {
        private readonly IEnumerable<string> _names = new[] { "migrate" };

        public IEnumerable<string> CommandNames
        {
            get { return _names; }
        }

        [Parameter("connection", "c", Position = 0)]
        public string Connection { get; set; }

        [Parameter("version", "v", Position = 1)]
        public long TargetVersion { get; set; }

        public void Execute()
        { }
    }
}