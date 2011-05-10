using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    /// <summary>
    /// Uses MEF to locate <see cref="ICommand"/> types in the application.
    /// </summary>
    public class MefCommandLocator : ICommandLocator
    {
        private readonly AggregateCatalog _catalog;
        private readonly CompositionContainer _container;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // MEF uses the private setter
        [ImportMany(typeof(ICommand), AllowRecomposition = true)]
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

            _container = new CompositionContainer(_catalog);

            // register ICommandLocator so that commands can take a dependency on it
            _container.ComposeExportedValue<ICommandLocator>(this);
            _container.ComposeParts(this);
        }

        public void RegisterCommand<TCommand>()
            where TCommand : ICommand
        {
            _catalog.Catalogs.Add(new TypeCatalog(typeof(TCommand)));
        }

        public virtual ICommand GetCommandByName(string commandName)
        {
            ICommand cmd = Commands
                .Where(c => string.Equals(c.Metadata.Name, commandName, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value)
                .FirstOrDefault();

            return cmd;
        }

        public virtual ICommandMetadata GetCommandMetadata(ICommand command)
        {
            ICommandMetadata meta = Commands
                .Where(c => c.Value == command)
                .Select(c => c.Metadata)
                .FirstOrDefault();

            return meta;
        }

        public virtual IDictionary<ICommand, ICommandMetadata> GetAllCommands()
        {
            return Commands.ToDictionary(x => x.Value, x => x.Metadata);
        }
    }
}