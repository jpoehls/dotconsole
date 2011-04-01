using System;
using System.Collections.Generic;

namespace DotConsole.Tests.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly IEnumerable<string> _names = new[] { "help", "?" };

        public IEnumerable<string> CommandNames
        {
            get { return _names; }
        }

        [Parameter(Position = 1)]
        public string TopicName { get; set; }

        [Parameter(Position = 2)]
        public string Verbosity { get; set; }

        public void Execute()
        { }
    }
}