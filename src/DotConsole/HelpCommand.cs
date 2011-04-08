using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotConsole
{
    [Command(DefaultCommandName, "?", IsDefault = true)]
    public class HelpCommand : IHelpCommand
    {
        public const string DefaultCommandName = "help";

        private const int IndentWidth = 2;
        private const int TabWidth = 4;

        public IEnumerable<string> ErrorMessages { get; set; }

        public ICommandLocator CommandLocator { get; set; }

        [Parameter("command", Position = 0)]
        public string CommandName { get; set; }

        public void Execute()
        {
            // todo: if there are error messages then show those first

            ICommand command = CommandLocator.GetCommand(CommandName);
            if (command != null)
            {
                // show help for this specific command
                WriteCommandHelp(command, Environment.GetCommandLineArgs()[0]);
            }
            else
            {
                // todo: write out the list of all available commands
                Console.WriteLine("Available commands:");
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
                syntax.Append(GetDefaultName(prop.Value.Names));
                syntax.Append(" ");
                syntax.Append(prop.Value.GetMetaName().ToUpperInvariant());

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
                .Select(x=>x.Description)
                .FirstOrDefault();

            return desc;
        }
        
        // todo: get rid of this. the help should list all available command and parameter names
        private static string GetDefaultName(IEnumerable<string> names)
        {
            return names.FirstOrDefault();
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

            int maxArgNameLength = properties.Max(x => /*x.Value.ShortName.Length*/ 0 + GetDefaultName(x.Value.Names).Length) + 4;

            foreach (var prop in properties)
            {
                Console.Write("".PadLeft(IndentWidth));
                Console.Write(string.Format("-{0}, --{1}", /*prop.Value.ShortName*/ "X", GetDefaultName(prop.Value.Names)).PadRight(maxArgNameLength + TabWidth));
                Console.WriteLine(GetParameterDescription(prop.Key));
            }
        }

        /// <summary>
        /// Writes out the help verbiage for the given  <see cref="ICommand" />.
        /// </summary>
        public void WriteCommandHelp(ICommand command, string executableName)
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
            Console.Write(executableName);
            Console.Write(" ");
            Console.Write(GetDefaultName(metadata.Names));
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

            int maxCommandNameLength = metadata.Values.Max(x => GetDefaultName(x.Names).Length);

            foreach (ICommand cmd in metadata.Keys)
            {
                var cmdMetadata = metadata[cmd];
                Console.Write("".PadLeft(2));
                Console.Write("{0}", GetDefaultName(cmdMetadata.Names).PadRight(maxCommandNameLength + TabWidth));
                Console.WriteLine(cmd.GetDescription());
            }
        }
    }
}