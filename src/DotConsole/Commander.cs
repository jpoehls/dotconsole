using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using DotConsole.Commands;

namespace DotConsole
{
    /// <summary>
    /// Bootstrapper for DotConsole. Handles coordinating all
    /// the necessary steps to route, compose, validate and execute <see cref="ICommand"/> instances.
    /// </summary>
    public class Commander
    {
        private readonly ICommandComposer _composer;
        private readonly ICommandLocator _locator;
        private readonly ICommandRouter _router;
        private readonly ICommandValidator _validator;
        private bool _builtinCommandsRegistered;

        public Commander(ICommandRouter router, ICommandValidator validator, ICommandComposer composer,
                         ICommandLocator locator)
        {
            _router = router;
            _validator = validator;
            _composer = composer;
            _locator = locator;
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
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the calling assembly.
        /// </summary>
        /// <returns></returns>
        public static Commander Standard()
        {
            return Standard(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the given assemblies.
        /// </summary>
        /// <param name="commandAssemblies">Assemblies that contain your <see cref="ICommand" /> implementations</param>
        /// <returns></returns>
        public static Commander Standard(params Assembly[] commandAssemblies)
        {
            var catalogs = new List<ComposablePartCatalog>();
            if (commandAssemblies != null)
            {
                catalogs.AddRange(commandAssemblies.Select(assembly => new AssemblyCatalog(assembly)));
            }

            return Standard(catalogs.ToArray());
        }

        /// <summary>
        /// Gets a <see cref="Commander" /> that works with
        /// commands in the given catalogs.
        /// </summary>
        /// <param name="catalogs">Catalogs that contain your <see cref="ICommand" /> implementations</param>
        /// <returns></returns>
        public static Commander Standard(params ComposablePartCatalog[] catalogs)
        {
            var locator = new MefCommandLocator(catalogs);
            var composer = new StandardComposer();
            var router = new StandardRouter(locator);
            var validator = new DataAnnotationValidator();
            return new Commander(router, validator, composer, locator);
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
            // register the built-in commands with the locator
            RegisterBuiltinCommands();

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
                    var helpCommand = (IHelpCommand) _locator.GetCommandByName(ReservedCommandNames.Help);
                    helpCommand.ErrorMessages = _validator.ErrorMessages;

                    command = helpCommand;
                }

                command.Execute();
            }
            else
            {
                // execute the help command if we no other command was found
                var helpCommand = (IHelpCommand) _locator.GetCommandByName(ReservedCommandNames.Help);

                string commandName = _router.GetCommandName(args);

                if (!string.IsNullOrWhiteSpace(commandName))
                {
                    helpCommand.ErrorMessages = new[] {"unknown command '" + commandName + "'"};
                }

                helpCommand.Execute();
            }
        }

        private void RegisterBuiltinCommands()
        {
            if (!_builtinCommandsRegistered)
            {
                _locator.RegisterCommand<MagicalHelpCommand>();
                _builtinCommandsRegistered = true;
            }
        }
    }
}