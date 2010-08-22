using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    /// <summary>
    /// Expects the command name to be the first argument.
    /// Filters out the first argument so it is excluded
    /// from the argument parsing later on.
    /// </summary>
    public class ArgumentBasedRouter : IRouter
    {
        /// <summary>
        /// Use the default command (if specified) if the
        /// command name specified in the arguments is invalid.
        /// If false then an error is thrown instead.
        /// </summary>
        public bool UseDefaultAsFallback { get; set; }

        /// <summary>
        /// Gets or sets the default command to run
        /// if no command is specified in the arguments.
        /// </summary>
        public Type DefaultCommand { get; set; }

        public ICommand GetCommand(IEnumerable<string> args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> FilterArguments(IEnumerable<string> args)
        {
            return args.Skip(1);
        }
    }
}