using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public class MefCommandLocator : ICommandLocator
    {
        private readonly AggregateCatalog _catalog;
        private readonly CompositionContainer _container;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // MEF uses the private setter
        [ImportMany(typeof(ICommand))]
        protected IList<Lazy<ICommand, ICommandMetadata>> Commands { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        protected AggregateCatalog Catalog
        {
            get { return _catalog; }
        }

        public MefCommandLocator()
            : this(new AssemblyCatalog(Assembly.GetCallingAssembly()))
        { }

        public MefCommandLocator(params ComposablePartCatalog[] catalogs)
        {
            Commands = new List<Lazy<ICommand, ICommandMetadata>>();
            _catalog = new AggregateCatalog();

            if (catalogs != null)
            {
                foreach (var catalog in catalogs)
                {
                    _catalog.Catalogs.Add(catalog);
                }
            }

            // always include our built-in help command
            _catalog.Catalogs.Add(new TypeCatalog(typeof(HelpCommand)));

            _container = new CompositionContainer(_catalog);
            _container.ComposeParts(this);
        }

        public virtual ICommand GetCommand(string commandName)
        {
            ICommand cmd = Commands
                .Where(c => string.Equals(c.Metadata.Name, commandName, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value)
                .FirstOrDefault();

            return cmd;
        }

        public ICommand GetDefaultCommand()
        {
            ICommand cmd = Commands
                .Where(c => c.Metadata.IsDefault)
                .Select(c => c.Value)
                .FirstOrDefault();

            return cmd;
        }

        public ICommandMetadata GetCommandMetadata(ICommand command)
        {
            ICommandMetadata meta = Commands
                .Where(c => c.Value == command)
                .Select(c => c.Metadata)
                .FirstOrDefault();

            return meta;
        }
    }
}