using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public class StandardRouter : ICommandRouter
    {
        private readonly ICommandComposer _composer;

        private readonly AggregateCatalog _catalog;
        private readonly CompositionContainer _container;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // MEF uses the private setter
        [ImportMany(typeof(ICommand))]
        protected IEnumerable<ICommand> Commands { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        protected AggregateCatalog Catalog
        {
            get { return _catalog; }
        }

        protected ICommandComposer Composer
        {
            get { return _composer; }
        }

        public StandardRouter()
            : this(new StandardComposer(), new AssemblyCatalog(Assembly.GetCallingAssembly()))
        {
        }

        public StandardRouter(ICommandComposer composer, params ComposablePartCatalog[] catalogs)
        {
            _composer = composer;
            Commands = new List<ICommand>();
            _catalog = new AggregateCatalog();

            foreach (var catalog in catalogs)
            {
                _catalog.Catalogs.Add(catalog);
            }

            _container = new CompositionContainer(_catalog);
            _container.ComposeParts(this);
        }

        protected virtual ICommand GetCommand(string commandName)
        {
            ICommand cmd = Commands
                .Where(c => c.CommandNames.Contains(commandName))
                .FirstOrDefault();

            return cmd;
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
                    var command = GetCommand(commandName);

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