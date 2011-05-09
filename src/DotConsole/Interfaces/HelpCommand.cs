using System.Collections.Generic;

namespace DotConsole
{
    public abstract class HelpCommand : ICommand
    {
        public const string HelpCommandName = "help";

        public ICommandLocator CommandLocator { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        #region ICommand Members

        public abstract void Execute();

        #endregion
    }
}