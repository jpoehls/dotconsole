using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class MagicalHelpCommandTests
    {
        [SetUp]
        public void Setup()
        {
            _expectedExecutableName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);

            _help = new MagicalHelpCommand();
            _help.CommandLocator = new MefCommandLocator(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
        }

        private MagicalHelpCommand _help;
        private string _expectedExecutableName;

        private class ConsoleOutput
        {
            private readonly string _stdOut, _stdErr;

            public string StdOut { get { return _stdOut; } }
            public string StdErr { get { return _stdErr; } }

            public ConsoleOutput(string stdOut, string stdErr)
            {
                _stdOut = stdOut;
                _stdErr = stdErr;
            }
        }

        /// <summary>
        /// Captures and returns the console output of a given <see cref="Action"/>.
        /// </summary>
        /// <param name="action"><see cref="Action"/> to capture the console output of.</param>
        /// <returns>The console output.</returns>
        private static ConsoleOutput CaptureConsoleOutput(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            // flush anything currently in the buffers
            Console.Out.Flush();
            Console.Error.Flush();

            // save the original streams so we can go back to them later
            TextWriter originalOutputStream = Console.Out;
            TextWriter originalErrorStream = Console.Error;

            using (var stdOutStream = new MemoryStream())
            using (var stdErrStream = new MemoryStream())
            using (var stdOutWriter = new StreamWriter(stdOutStream))
            using (var stdErrWriter = new StreamWriter(stdErrStream))
            {
                // start outputting to our capture streams
                Console.SetOut(stdOutWriter);
                Console.SetError(stdErrWriter);

                // run the action
                action();

                // flush the buffers
                Console.Out.Flush();
                Console.Error.Flush();

                // switch back to the original output streams
                Console.SetOut(originalOutputStream);
                Console.SetError(originalErrorStream);

                // get and return the text in the capture streams
                stdOutStream.Seek(0, SeekOrigin.Begin);
                stdErrStream.Seek(0, SeekOrigin.Begin);
                using (var stdOutReader = new StreamReader(stdOutStream))
                using (var stdErrReader = new StreamReader(stdErrStream))
                {
                    var output = new ConsoleOutput(stdOutReader.ReadToEnd(), stdErrReader.ReadToEnd());
                    return output;
                }
            }
        }

        [Test]
        public void Execute_should_output_list_of_commands_when_CommandName_property_is_null()
        {
            // arrange
            string expectedOutput = "list of commands:" + Environment.NewLine +
                                    Environment.NewLine +
                                    " help" + Environment.NewLine +
                                    " testhelp" + Environment.NewLine +
                                    " testmigrate" + Environment.NewLine;

            // act
            var output = CaptureConsoleOutput(() => _help.Execute());

            // assert
            Assert.AreEqual(expectedOutput, output.StdOut);
        }

        [Test]
        public void Execute_should_output_error_messages_to_stderr()
        {
            // arrange
            string expectedOutput = _expectedExecutableName + ": error message goes here" + Environment.NewLine;
            _help.ErrorMessages = new[] { "error message goes here" };

            // act
            var output = CaptureConsoleOutput(() => _help.Execute());

            // assert
            Assert.AreEqual(expectedOutput, output.StdErr);
        }

        [Test]
        public void Execute_should_include_command_name_in_error_message_output()
        {
            // arrange
            string expectedOutput = _expectedExecutableName + " testmigrate: error message goes here" + Environment.NewLine;
            _help.CommandName = "testmigrate";
            _help.ErrorMessages = new[] { "error message goes here" };

            // act
            var output = CaptureConsoleOutput(() => _help.Execute());

            // assert
            Assert.AreEqual(expectedOutput, output.StdErr);
        }


        [Test]
        public void Execute_should_output_list_of_commands_when_there_are_error_messages()
        {
            // arrange
            string expectedOutput = "list of commands:" + Environment.NewLine +
                                    Environment.NewLine +
                                    " help" + Environment.NewLine +
                                    " testhelp" + Environment.NewLine +
                                    " testmigrate" + Environment.NewLine;

            _help.ErrorMessages = new[] { "error message goes here" };

            // act
            var output = CaptureConsoleOutput(() => _help.Execute());

            // assert
            Assert.AreEqual(expectedOutput, output.StdOut);
        }

        [Test]
        public void Execute_should_throw_exception_if_CommandLocator_property_is_null()
        {
            // arrange
            _help.CommandLocator = null;

            // act
            var ex = Assert.Throws<InvalidOperationException>(() => _help.Execute(), "No exception was thrown.");

            // assert
            Assert.IsTrue(ex.Message.Contains("CommandLocator property"),
                          "The error message didn't mention the CommandLocator property.");
        }
    }
}