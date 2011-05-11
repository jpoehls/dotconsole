using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using DotConsole.Commands;
using DotConsole.Routing;

namespace DotConsole
{
    /// <summary>
    /// Bootstrapper for DotConsole. Handles coordinating all
    /// the necessary steps to route, compose, validate and execute <see cref="ICommand"/> instances.
    /// </summary>
    public class Commander
    {
        private readonly ICommandLocator _locator;
        private readonly ICommandRouter _router;
        private ICommandComposer _composer;
        private ICommandValidator _validator;

        /// <summary>
        /// Initializes a <see cref="Commander"/> that works with
        /// commands in the calling assembly.
        /// </summary>
        public Commander()
            : this(Assembly.GetCallingAssembly())
        { }

        /// <summary>
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the given <see cref="Assembly"/>'s.
        /// </summary>
        /// <param name="commandAssemblies">Assemblies that contain your <see cref="ICommand" /> implementations</param>
        public Commander(params Assembly[] commandAssemblies)
            : this((IEnumerable<Assembly>)commandAssemblies)
        { }

        /// <summary>
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the given <see cref="Assembly"/>'s.
        /// </summary>
        /// <param name="commandAssemblies">Assemblies that contain your <see cref="ICommand" /> implementations</param>
        public Commander(IEnumerable<Assembly> commandAssemblies)
            : this(commandAssemblies.Select(assembly => new AssemblyCatalog(assembly)))
        { }

        /// <summary>
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the given <see cref="ComposablePartCatalog"/>'s.
        /// </summary>
        /// <param name="catalogs">Catalogs that contain your <see cref="ICommand" /> implementations</param>
        public Commander(params ComposablePartCatalog[] catalogs)
            : this((IEnumerable<ComposablePartCatalog>)catalogs)
        { }

        /// <summary>
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the given <see cref="ComposablePartCatalog"/>'s.
        /// </summary>
        /// <param name="catalogs">Catalogs that contain your <see cref="ICommand" /> implementations</param>
        public Commander(IEnumerable<ComposablePartCatalog> catalogs)
        {
            _validator = new DataAnnotationValidator();
            _composer = new StandardComposer();
            _locator = new MefCommandLocator(catalogs);
            _router = new FirstArgumentRouter(_locator);

            // register the built-in commands with the locator
            RegisterBuiltinCommands();
        }

        /// <summary>
        /// Get/sets the name of the application as it should
        /// be used in help command output.
        /// </summary>
        public static string ApplicationName { get; set; }

        /// <summary>
        /// Gets the <see cref="ICommandLocator"/> used to find <see cref="ICommand"/> types.
        /// </summary>
        public ICommandLocator Locator
        {
            get { return _locator; }
        }

        /// <summary>
        /// Gets/sets the <see cref="ICommandValidator"/> used to validate the <see cref="ICommand"/>
        /// parameters before the command is executed.
        /// </summary>
        public ICommandValidator Validator
        {
            get { return _validator; }
            set
            {
                if (value == null)
                    return;

                _validator = value;
            }
        }

        /// <summary>
        /// Gets/sets the <see cref="ICommandComposer"/> used to populate the <see cref="ICommand"/>
        /// parameters from a list of arguments (typically from the command line).
        /// </summary>
        public ICommandComposer Composer
        {
            get { return _composer; }
            set
            {
                if (value == null)
                    return;

                _composer = value;
            }
        }

        /// <summary>
        /// Routes and executes the appropriate <see cref="ICommand" /> 
        /// using <see cref="Environment.GetCommandLineArgs()" />.
        /// </summary>
        public virtual void Run()
        {
            // We are skipping the first arg since it contains
            // the executable name (not an actual argument).
            Run(Environment.GetCommandLineArgs().Skip(1));
        }

        /// <summary>
        /// Routes and executes the appropriate <see cref="ICommand" />
        /// using the given arguments.
        /// </summary>
        /// <param name="args">Arguments to use to route and execute the command.</param>
        public virtual void Run(IEnumerable<string> args)
        {
            ICommand command = _router.Route(args);

            if (command != null)
            {
                // Note that we are skipping the first arg since
                // it contains the command name (not an actual argument).
                _composer.ComposeParameters(command, args.Skip(1));

                if (!_validator.ValidateParameters(command))
                {
                    // get the help command from the router so that
                    // we will use any custom help command the user has added
                    var helpCommand = (IHelpCommand)_locator.GetCommandByName(ReservedCommandNames.Help);
                    helpCommand.ErrorMessages = _validator.ErrorMessages;

                    command = helpCommand;
                }

                command.Execute();
            }
            else
            {
                // execute the help command if we no other command was found
                var helpCommand = (IHelpCommand)_locator.GetCommandByName(ReservedCommandNames.Help);

                string commandName = _router.GetCommandName(args);

                if (!string.IsNullOrWhiteSpace(commandName))
                {
                    helpCommand.ErrorMessages = new[] { "unknown command '" + commandName + "'" };
                }

                helpCommand.Execute();
            }
        }

        /// <summary>
        /// Adds our built-in commands to the locator.
        /// </summary>
        private void RegisterBuiltinCommands()
        {
            _locator.RegisterCommand<MagicalHelpCommand>();
        }
    }
}