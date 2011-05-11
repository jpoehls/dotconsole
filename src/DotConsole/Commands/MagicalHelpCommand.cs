using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using DotConsole.Helpers;

namespace DotConsole.Commands
{
    /// <summary>
    /// Generates help output by introspecting over all
    /// known commands in the <see cref="ICommandLocator"/>
    /// and inspecting the parameters.
    /// </summary>
    [Command(ReservedCommandNames.Help)]
    [Description("Show help for a given command or a help overview.")]
    internal class MagicalHelpCommand : IHelpCommand
    {
        private readonly ICommandLocator _locator;
        private const int IndentWidth = 1;
        private const int TabWidth = 3;
        private const string ArgFlagPrefix = "-";
        private const string ArgNamePrefix = "--";

        private static readonly string ExecutableName;

        public IEnumerable<string> ErrorMessages { get; set; }

        static MagicalHelpCommand()
        {
            ExecutableName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
        }

        [ImportingConstructor]
        public MagicalHelpCommand(ICommandLocator locator)
        {
            if (locator == null)
                throw new ArgumentNullException("locator");

            _locator = locator;
        }

        /// <summary>
        /// Gets/sets the name of the command to display help information for.
        /// </summary>
        [Parameter("command", MetaName = "command_name", Position = 0)]
        public string CommandName { get; set; }

        public void Execute()
        {
            ICommand command = _locator.GetCommandByName(CommandName);
            ICommandMetadata commandMeta = (command != null)
                ? _locator.GetCommandMetadata(command)
                : null;

            if (ErrorMessages != null)
            {
                // write each error message to StdErr including the executable name and command name
                foreach (var msg in ErrorMessages)
                {
                    Console.Error.Write(ExecutableName);
                    if (commandMeta != null)
                    {
                        Console.Error.Write(" {0}", commandMeta.Name);
                    }
                    Console.Error.WriteLine(": {0}", msg);
                }
            }

            if (!string.IsNullOrWhiteSpace(Commander.ApplicationName))
            {
                Console.WriteLine(Commander.ApplicationName);
                Console.WriteLine();
            }

            if (command != null)
            {
                WriteCommandHelp(command, commandMeta);
            }
            else
            {
                WriteCommandList();
            }
        }

        /// <summary>
        /// Writes out a list of all available commands and their descriptions.
        /// </summary>
        public void WriteCommandList()
        {
            Console.WriteLine("list of commands:");
            Console.WriteLine();

            var commands = _locator.GetAllCommands();
            int maxCommandNameLength = commands.Values.Max(x => x.Name.Length);

            foreach (var cmd in commands.OrderBy(x => x.Value.Name))
            {
                Console.Write(new string(' ', IndentWidth));
                Console.Write(cmd.Value.Name);

                var desc = cmd.Key.GetDescription();
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    Console.Write(new string(' ', maxCommandNameLength - cmd.Value.Name.Length));
                    Console.Write(new string(' ', TabWidth));
                    Console.Write(desc);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Writes out the help and usage information for the given <see cref="ICommand" />.
        /// </summary>
        public void WriteCommandHelp(ICommand command, ICommandMetadata metadata)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            var argSyntax = GetArgumentSyntax(command);

            Console.Write("{0} {1}", ExecutableName, metadata.Name);
            if (!string.IsNullOrWhiteSpace(argSyntax))
            {
                Console.WriteLine(argSyntax);
            }

            var cmdDesc = command.GetDescription();
            if (!string.IsNullOrWhiteSpace(cmdDesc))
            {
                Console.WriteLine();
                Console.WriteLine(cmdDesc);
            }

            var parameters = command.GetParameters();
            if (parameters.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("options:");
                foreach (var param in parameters)
                {
                    // todo: write out the param info
                }
            }
        }

        /// <summary>
        /// Builds and returns the argument syntax for the given <see cref="ICommand" />.
        /// </summary>
        public string GetArgumentSyntax(ICommand command)
        {
            //  SAMPLE OUTPUT
            //
            //  -requiredArg req_value_name [-optionalArg1 value_name] [-optionalArg2 value_name]

            var properties = command.GetParameters();

            var syntax = new StringBuilder();

            int count = 0;
            foreach (var prop in properties)
            {
                bool optional = !prop.Key.IsRequired();
                if (optional)
                {
                    syntax.Append("[");
                }

                syntax.Append("--");
                syntax.Append(prop.Value.Name);
                syntax.Append(" ");
                syntax.Append(prop.Value.MetaName);

                if (optional)
                {
                    syntax.Append("]");
                }

                //  if not the last one, add a space
                if (count < properties.Count - 1)
                {
                    syntax.Append(" ");
                }

                count++;
            }

            return syntax.ToString();
        }
    }
}