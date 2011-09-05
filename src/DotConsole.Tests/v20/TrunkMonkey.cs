using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace DotConsole.Tests.v20
{
    // dnm.exe --ConfigFile "value here" --Environment LOCAL --ResetDatabase --WarnOnHashChange
    // dnm.exe -rw --ConfigFile "value here" --Environment LOCAL
    // dnm.exe -rwc "value here" --Environment LOCAL
    // dnm.exe "value here" DEV -rw
    // dnm.exe LOCAL

    // ConfigFile - Position: 1
    // Environment - Position: 2
    // ResetDatabase


    public class RawArgSet : Dictionary<string, List<string>>
    {
        public List<string> AnonymousValues { get; private set; }

        public RawArgSet()
        {
            AnonymousValues = new List<string>();
        }

        public static RawArgSet Parse(IEnumerable<string> args)
        {
            var set = new RawArgSet();
            string lastKey = null;

            foreach (var cmdLine in args)
            {
                if (cmdLine.StartsWith("--"))
                {
                    lastKey = cmdLine.Substring(2).ToLowerInvariant();
                    if (!set.ContainsKey(lastKey))
                        set.Add(lastKey, new List<string>());
                }
                else if (cmdLine.StartsWith("-"))
                {
                    foreach (var c in cmdLine.Substring(1))
                    {
                        lastKey = c.ToString();
                        if (!set.ContainsKey(lastKey))
                            set.Add(lastKey, new List<string>());
                    }
                }
                else
                {
                    if (lastKey == null)
                    {
                        set.AnonymousValues.Add(cmdLine);
                    }
                    else
                    {
                        set[lastKey].Add(cmdLine);
                    }
                }
            }

            return set;
        }
    }

    public class CommandLocator
    {
        [ImportMany(typeof(ICommand))]
        protected List<Lazy<ICommand, ICommandMetadata>> Commands { get; set; }

        public CommandLocator()
        {
            var catalog = new AggregateCatalog();
            //catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new TypeCatalog(typeof(InvokeCommand)));

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public ICommand GetDefaultCommand()
        {
            var found = Commands.FirstOrDefault(x => x.Metadata.Default);
            if (found != null)
            {
                return found.Value;
            }
            return null;
        }

        public ICommand GetCommandByName(string commandName)
        {
            // try to match the command name in the metadata first
            ICommand cmd = Commands
                .Where(x => x.Metadata.Name == commandName)
                .Select(x => x.Value)
                .FirstOrDefault();

            // then try to match based on the command's type name
            if (cmd == null)
                cmd = Commands
                    .Select(x => new
                        {
                            x.Value,
                            TypeName = x.Value.GetType().Name
                        })
                    .Where(x => x.TypeName == commandName || x.TypeName == commandName + "Command")
                    .Select(x => x.Value)
                    .FirstOrDefault();

            return cmd;
        }
    }

    public class CommandFiller
    {
        public void Fill(ICommand cmd, RawArgSet args)
        {
            
        }
    }

    public class CommandValidator
    {
        public void Validate(ICommand cmd)
        {
            
        }
    }

    public static class Commander
    {


        public static void Run()
        {
            var args = Environment.GetCommandLineArgs().Skip(1);
            var set = RawArgSet.Parse(args);
            var cmd = (new CommandLocator()).GetCommandByName(set.AnonymousValues[0]);
        }
    }

    public class RawArg
    {

        [Fact]
        public void Tests()
        {
//            string[] line = new string[]
//                                {
//                                    "abcdef",
//                                    "-rw",
//                                    "--ConfigFile",
//                                    "\"value here\"",
//                                    "--Environment",
//                                    "LOCAL",
//                                    "--ConfigFile",
//                                    "another value"
//                                };
//
//            var results = Parse(line);
//            PrintResults(results);
//
//            Assert.True(results.ContainsKey("r"), "No 'r' key.");
//            Assert.True(results.ContainsKey("w"), "No 'w' key.");
//            Assert.True(results.ContainsKey("ConfigFile"), "No 'ConfigFile' key.");
//            Assert.True(results.ContainsKey("Environment"), "No 'Environment' key.");
//
//            Assert.Empty(results["r"]);
//            Assert.Empty(results["w"]);
//            Assert.True(results["ConfigFile"].Count == 2, "No 'ConfigFile' value.");
//            Assert.Equal("\"value here\"", results["ConfigFile"][0]);
//            Assert.Equal("another value", results["ConfigFile"][1]);
//            Assert.True(results["Environment"].Count == 1, "No 'Environment' value.");
//            Assert.Equal("LOCAL", results["Environment"][0]);
        }

        [Fact]
        public void Should_find_command_by_name()
        {
//            var cmd = GetCommand(new Dictionary<string, List<string>>()
//                                     {
//                                         {
//                                             string.Empty, new List<string>()
//                                                               {
//                                                                   "Invoke"
//                                                               }
//                                             }
//                                     });
//            Assert.IsType<InvokeCommand>(cmd);
        }

        private void PrintResults(Dictionary<string, List<string>> results)
        {
            foreach (var key in results.Keys)
            {
                if (key.Length == 0)
                    Console.Write("ANONYMOUS");
                else if (key.Length == 1)
                    Console.Write("-");
                else
                    Console.Write("--");

                Console.WriteLine(key);

                foreach (var value in results[key])
                {
                    Console.WriteLine("  " + value);
                }
            }
        }
    }


    public class ArgumentAttribute : Attribute
    {
        public ArgumentAttribute(string desc)
        {
            Description = desc;
            Position = -1;
        }

        /// <summary>
        /// Long argument name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviated name that is friendly for the command line.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Used in the syntax help text as the placeholder for the value.
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// If the argument is not passed in with a name then it will
        /// be matched based on its position in the array of unnamed arguments.
        /// </summary>
        public int Position { get; set; }

        public string Description { get; set; }
    }

    public interface ICommandMetadata
    {
        string Name { get; }
        bool Default { get; }
        string Description { get; }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandAttribute : ExportAttribute, ICommandMetadata
    {
        public string Name { get; set; }
        public bool Default { get; set; }
        public string Description { get; set; }

        public CommandAttribute()
            : base(typeof(ICommand))
        { }
    }

    public interface ICommand
    {
        void Run();
    }

    [Command(Default = true)]
    public class InvokeCommand : ICommand
    {
        [Required()]
        [Argument("Name of the person running the sample.",
            Position = 1,
            ValueName = "persons_name")]
        public string ConfigFile { get; set; }

        public void Run()
        {

        }
    }

    public class TrunkMonkey
    {
        [ImportMany(typeof(ICommand))]
        public IList<Lazy<ICommand, ICommandMetadata>> Commands { get; set; }

        public TrunkMonkey()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public Type GetCommandType(IEnumerable<string> args)
        {
            return null;
        }
    }
}
