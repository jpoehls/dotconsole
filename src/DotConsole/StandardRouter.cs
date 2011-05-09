using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    /// <summary>
    /// Determines which <see cref="ICommand"/> should execute based
    /// on a list of arguments (typically from the command line).
    /// </summary>
    public class StandardRouter : ICommandRouter
    {
        private readonly ICommandLocator _locator;

        public ICommandLocator Locator
        {
            get { return _locator; }
        }

        public StandardRouter(ICommandLocator locator)
        {
            _locator = locator;
        }

        public virtual ICommand Route(IEnumerable<string> args)
        {
            ICommand command = null;

            if (args != null)
            {
                var commandName = GetCommandName(args);
                if (!string.IsNullOrWhiteSpace(commandName))
                {
                    command = _locator.GetCommandByName(commandName);
                }
            }

            return command;
        }

        public string GetCommandName(IEnumerable<string> args)
        {
            string name = null;
            if (args != null)
            {
                name = args.FirstOrDefault();
            }
            return name;
        }
    }
}