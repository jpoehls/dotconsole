﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotConsole
{
    /// <summary>
    /// Generates help output by introspecting over all
    /// known commands in the <see cref="ICommandLocator"/>
    /// and inspecting the parameters.
    /// </summary>
    [Command(HelpCommandName, IsDefault = true)]
    public class MagicalHelpCommand : HelpCommand
    {
        private const int IndentWidth = 1;
        private const int TabWidth = 3;

        private static readonly string ExecutableName;

        static MagicalHelpCommand()
        {
            ExecutableName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
        }

        [Parameter(0)]
        public string CommandName { get; set; }

        public override void Execute()
        {
            if (CommandLocator == null)
            {
                throw new InvalidOperationException(
                    "CommandLocator property is null. Help command requires a non-null CommandLocator instance.");
            }

            ICommand command = CommandLocator.GetCommandByName(CommandName);
            ICommandMetadata commandMeta = (command != null)
                ? CommandLocator.GetCommandMetadata(command)
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

            if (command != null)
            {
                WriteCommandHelp(command);
            }
            else
            {
                WriteCommandList();
            }
        }

        private void WriteCommandList()
        {
            Console.WriteLine("list of commands:");
            Console.WriteLine();

            var commands = CommandLocator.GetAllCommands()
                .OrderBy(x => x.Value.Name);
            foreach (var cmd in commands)
            {
                Console.WriteLine("{0}{1}{2}{3}", new string(' ', IndentWidth), cmd.Value.Name,
                    new string(' ', TabWidth), cmd.Key.GetDescription());
            }
        }

        /// <summary>
        /// Returns True/False whether the given property is optional.
        /// </summary>
        private static bool IsOptional(ICustomAttributeProvider property)
        {
            bool optional = property.GetCustomAttributes(typeof(RequiredAttribute), false)
                                .Count() == 0;
            return optional;
        }

        /// <summary>
        /// Writes out the argument syntax for the given <see cref="ICommand" />.
        /// </summary>
        public void WriteArgumentSyntax(ICommand command)
        {
            //  SAMPLE OUTPUT
            //
            //  -requiredArg req_value_name [-optionalArg1 value_name] [-optionalArg2 value_name]

            var properties = command.GetParameters();

            var syntax = new StringBuilder();

            int count = 0;
            foreach (var prop in properties)
            {
                bool optional = IsOptional(prop.Key);
                if (optional)
                {
                    syntax.Append("[");
                }

                syntax.Append("--");
                syntax.Append(prop.Value);
                syntax.Append(" ");
                syntax.Append(prop.Value.MetaName.ToUpperInvariant());

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

            Console.Write(syntax.ToString());
        }

        private static string GetParameterDescription(PropertyInfo property)
        {
            var desc = property.GetCustomAttributes(typeof(DescriptionAttribute), true)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault();

            return desc;
        }

        /// <summary>
        /// Writes out the list of arguments for the given <see cref="ICommand" />.
        /// </summary>
        public void WriteArgumentList(ICommand command)
        {
            //  SAMPLE OUTPUT
            //
            //  Options:
            //    -f, -firstArg       description of first argument
            //    -s, -secondArg      description of second argument

            Console.WriteLine(string.Empty);
            Console.WriteLine("Options:");

            var properties = command.GetParameters();

            int maxArgNameLength = properties.Max(x => /*x.Value.ShortName.Length*/ 0 + x.Value.Name.Length) + 4;

            foreach (var prop in properties)
            {
                Console.Write("".PadLeft(IndentWidth));
                Console.Write(string.Format("-{0}, --{1}", /*prop.Value.ShortName*/ "X", prop.Value.Name).PadRight(maxArgNameLength + TabWidth));
                Console.WriteLine(GetParameterDescription(prop.Key));
            }
        }

        /// <summary>
        /// Writes out the help verbiage for the given  <see cref="ICommand" />.
        /// </summary>
        public void WriteCommandHelp(ICommand command)
        {
            //  SAMPLE OUTPUT
            //
            //  Usage: db.exe commandName [ARGUMENT SYNTAX]
            //  
            //  Options:
            //    -f, -firstArg       description of first argument
            //    -s, -secondArg      description of second argument       

            var metadata = CommandLocator.GetCommandMetadata(command);

            Console.Write("Usage: ");
            Console.Write(ExecutableName);
            Console.Write(" ");
            Console.Write(metadata.Name);
            Console.Write(" ");

            WriteArgumentSyntax(command);

            Console.WriteLine(string.Empty);
            WriteArgumentList(command);
        }

        /// <summary>
        /// Writes out a list of the given <see cref="ICommand" /> names and descriptions.
        /// </summary>
        public void WriteCommandList(IEnumerable<ICommand> commands)
        {
            //  SAMPLE OUTPUT
            //
            //  Commands:
            //    firstCommand        description of first command
            //    secondCommand       description of second command

            Console.WriteLine(string.Empty);
            Console.WriteLine("Available commands:");

            var metadata = commands.ToDictionary(cmd => cmd, cmd => CommandLocator.GetCommandMetadata(cmd));

            int maxCommandNameLength = metadata.Values.Max(x => x.Name.Length);

            foreach (ICommand cmd in metadata.Keys)
            {
                var cmdMetadata = metadata[cmd];
                Console.Write("".PadLeft(2));
                Console.Write("{0}", cmdMetadata.Name.PadRight(maxCommandNameLength + TabWidth));
                Console.WriteLine(cmd.GetDescription());
            }
        }
    }
}