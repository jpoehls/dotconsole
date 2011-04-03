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

        protected ICommandLocator Locator
        {
            get { return _locator; }
        }

        public StandardRouter(ICommandLocator locator, ICommandComposer composer)
        {
            _locator = locator;
            _composer = composer;
        }

        public virtual ICommand Route()
        {
            // Note that we are skipping the first arg since
            // it contains the executable name (not an actual argument).
            return Route(Environment.GetCommandLineArgs().Skip(1));
        }

        public virtual ICommand Route(IEnumerable<string> args)
        {
            if (args != null)
            {
                string commandName = args.FirstOrDefault();
                if (commandName != null)
                {
                    var command = _locator.GetCommand(commandName);

                    // Note that we are skipping the first arg since
                    // it contains the command name (not an actual argument).
                    _composer.ComposeParameters(command, args.Skip(1));
                    return command;
                }
            }

            return null;
        }
    }
}