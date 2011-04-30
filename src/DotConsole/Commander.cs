using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public class Commander
    {
        private readonly ICommandRouter _router;
        private readonly ICommandValidator _validator;

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

            var locator = new MefCommandLocator(catalogs.ToArray());
            var composer = new StandardComposer();
            var router = new StandardRouter(locator, composer);
            var validator = new DataAnnotationValidator();
            return new Commander(router, validator);
        }

        public Commander(ICommandRouter router, ICommandValidator validator)
        {
            _router = router;
            _validator = validator;
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
            var command = _router.Route(args);
            if (command != null)
            {
                if (!_validator.ValidateParameters(command))
                {
                    // get the help command from the router so that
                    // we will use any custom help command the user has added
                    command = _router.Locator.GetCommandByName(HelpCommand.HelpCommandName);
                    if (command is HelpCommand)
                    {
                        ((HelpCommand)command).ErrorMessages = _validator.ErrorMessages;
                    }
                }

                command.Execute();
            }
        }
    }
}
