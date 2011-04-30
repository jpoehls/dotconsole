using System.Collections.Generic;

namespace DotConsole
{
    public abstract class HelpCommand : ICommand
    {
        public const string HelpCommandName = "help";

        public abstract ICommandLocator CommandLocator { get; set; }
        public abstract IEnumerable<string> ErrorMessages { get; set; }
        public abstract void Execute();
    }
}