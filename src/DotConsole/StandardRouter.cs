using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    public class StandardRouter : ICommandRouter
    {
        private readonly ICommandComposer _composer;
        private readonly ICommandLocator _locator;

        protected ICommandComposer Composer
        {
            get { return _composer; }
        }

        public ICommandLocator Locator
        {
            get { return _locator; }
        }

        public StandardRouter(ICommandLocator locator, ICommandComposer composer)
        {
            _locator = locator;
            _composer = composer;
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

                    if (command != null)
                    {
                        // Note that we are skipping the first arg since
                        // it contains the command name (not an actual argument).
                        _composer.ComposeParameters(command, args.Skip(1));
                    }
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