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
        protected IEnumerable<ICommand> Commands { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        protected AggregateCatalog Catalog
        {
            get { return _catalog; }
        }

        public MefCommandLocator() : this(new AssemblyCatalog(Assembly.GetCallingAssembly()))
        {}

        public MefCommandLocator(params ComposablePartCatalog[] catalogs)
        {
            Commands = new List<ICommand>();
            _catalog = new AggregateCatalog();

            foreach (var catalog in catalogs)
            {
                _catalog.Catalogs.Add(catalog);
            }

            _container = new CompositionContainer(_catalog);
            _container.ComposeParts(this);
        }

        public virtual ICommand GetCommand(string commandName)
        {
            ICommand cmd = Commands
                .Where(c => c.CommandNames.Contains(commandName))
                .FirstOrDefault();

            return cmd;
        }
    }
}