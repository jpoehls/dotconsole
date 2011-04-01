using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DotConsole.Tests
{
    public class OptionParser
    {
        //public List<Option> Options { get; set; }

        public static IEnumerable<ParsedOption> Parse(IEnumerable<string> args, params Option[] supportedOptions)
        {
            return new List<ParsedOption>();
        }
    }

    public class ParsedOption
    {
        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
        public IEnumerable<KeyValuePair<string, string>> KeyedValues { get; set; }
    }

    public class Option
    {
        public Option(params string[] names)
        {
            ShortNames = new List<char>();
            LongNames = new List<string>();

            foreach (var name in names)
            {
                if (name.Length == 1)
                    ShortNames.Add(name[0]);
                else if (name.Length > 1)
                    LongNames.Add(name);
            }
        }

        public List<char> ShortNames { get; set; }
        public List<string> LongNames { get; set; }
        public string DestinationName { get; set; }
        public bool IsConstant { get; set; }
        public string DefaultValue { get; set; }
        public Func<string, string> ValueConverter { get; set; }
        public Action<string> Callback { get; set; }
    }

    public class OptionParserTests
    {
        [Fact]
        public void TestCasePing1()
        {
            // arrange
            var args = new[]
                           {
                               "ping.exe",
                               "-w",
                               "100",
                               "-k",
                               "host_list"
                           };

            var options = new[]
                              {
                                  new Option("w"),
                                  new Option("k")
                              };

            // act
            var output = OptionParser.Parse(args, options);

            // assert
            Assert.True(output.Count(o => o.Name == "w") == 0, "missing 'w' arg");
            Assert.True(output.First(w => w.Name == "w").Values.First() == "100", "wrong value for 'w'");
            Assert.True(output.Count(o => o.Name == "k") == 0, "missing 'k' arg");
            Assert.True(output.First(w => w.Name == "k").Values.First() == "100", "wrong value for 'k'");
        }

        #region Test Commands
        
        public class HelpCommand : ICommand
        {
            public IEnumerable<string> CommandNames { get; set; }

            [Parameter]
            public string TopicName { get; set; }

            public void Execute()
            {
                Console.WriteLine("Help command executed!");
            }
        }

        public class HelpCommandArgs
        {
            public string TopicName { get; set; }
        }


        #endregion

        [Fact]
        public void ShouldRouteToCorrectCommandAndFireCallbackWithRemainingArgs()
        {
            var cmdRouter = new CommandRouter();

            bool callbackFired = false;
            cmdRouter.AddCommand("help", (IEnumerable<string> args) =>
                                             {
                                                 callbackFired = true;
                                                 Console.WriteLine("help command was run!");

                                                 Assert.True(args.Count() == 1);
                                                 Assert.True(args.First() == "commit");
                                             });

            var testArgs = new[]
                               {
                                   "help",
                                   "commit"
                               };

            cmdRouter.Route(testArgs);

            Assert.True(callbackFired, "callback was not fired");
        }

        [Fact]
        public void Discovery()
        {
            var cmdRouter = new CommandRouter();

            bool callbackFired = false;
            cmdRouter.AddCommand<HelpCommandArgs>("help", (HelpCommandArgs args) =>
            {
                callbackFired = true;
                Console.WriteLine("help command was run!");

                Assert.True(args.TopicName == "commit");
            });

            var testArgs = new[]
                               {
                                   "help",
                                   "commit"
                               };

            cmdRouter.Route(testArgs);

            Assert.True(callbackFired, "callback was not fired");
        }
    }

    public class CommandRouter
    {
        public CommandRouter()
        {
            _commands = new Dictionary<string, Tuple<Type, object>>();
        }

        private readonly Dictionary<string, Tuple<Type, object>> _commands;

        public void AddCommand(string name, Action<IEnumerable<string>> callback)
        {
            _commands.Add(name, new Tuple<Type, object>(null, callback));
        }

        public void AddCommand<TArgs>(string name, Action<TArgs> callback)
        {
            _commands.Add(name, new Tuple<Type, object>(typeof(TArgs), callback));
        }

        public void Route(string[] testArgs)
        {
            string commandName = testArgs[0];

            var callback = _commands[commandName];
            if (callback.Item1 == null)
                (callback.Item2 as Action<IEnumerable<string>>)(testArgs.Skip(1));
            else
                (callback.Item2 as Action<callback.Item1>)(new OptionParserTests.HelpCommandArgs());
        }
    }
}