using System.Collections.Generic;
using System.Linq;

namespace DotConsole.Routing
{
    /// <summary>
    /// Finds and returns the <see cref="ICommand" />
    /// whose name matches the first argument in a given list.
    /// </summary>
    internal class FirstArgumentRouter : ICommandRouter
    {
        private readonly ICommandLocator _locator;

        public ICommandLocator Locator
        {
            get { return _locator; }
        }

        public FirstArgumentRouter(ICommandLocator locator)
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