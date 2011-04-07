using System;
using System.Linq;
using DotConsole.Tests.Commands;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class StandardRouterTests
    {
        private StandardRouter _router;

        [SetUp]
        public void Setup()
        {
            _router = new StandardRouter(new MefCommandLocator(), new StandardComposer());
        }

        [Test]
        public void Route_should_return_the_TestTestHelpCommand_instead_of_the_builtin_help_command_when_the_help_command_name_is_specified()
        {
            
        }

        [Test]
        public void Route_should_return_the_TestTestHelpCommand_instead_of_the_builtin_help_command_as_the_default_command()
        {

        }

        [Test]
        public void Route_should_return_the_default_command_if_no_command_matches_the_first_arg()
        {
            // arrange
            var testArgs = new[] { "some unknown command" };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNotNull(command, "command is null - routing failed");
            Assert.IsInstanceOf(typeof(TestHelpCommand), command, "wrong command returned");
        }

        [Test]
        public void Route_should_return_the_default_command_if_args_is_empty()
        {
            // arrange
            var testArgs = new string[] { };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNotNull(command, "command is null - routing failed");
            Assert.IsInstanceOf(typeof(TestHelpCommand), command, "wrong command returned");
        }

        [Test]
        public void Route_should_return_the_default_command_if_args_is_null()
        {
            // arrange
            string[] testArgs = null;

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNotNull(command, "command is null - routing failed");
            Assert.IsInstanceOf(typeof(TestHelpCommand), command, "wrong command returned");
        }

        [Test]
        public void Route_should_return_command_with_a_name_matching_the_first_arg()
        {
            // arrange
            var testArgs = new[] { "help" };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNotNull(command, "command is null - routing failed");
            Assert.IsInstanceOf(typeof(TestHelpCommand), command, "wrong command returned");
        }

        [Test]
        public void Route_should_return_command_with_a_name_matching_the_first_arg2()
        {
            // arrange
            var testArgs = new[] { "?" };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNotNull(command, "command is null - routing failed");
            Assert.IsInstanceOf(typeof(TestHelpCommand), command, "wrong command returned");
        }

        [Test]
        [IgnoreAttribute("This won't work because we have a default command used by the other tests.")]
        public void Route_should_return_null_if_no_command_is_found_and_there_is_no_default()
        {
            // arrange
            var testArgs = new[] { "badcommand" };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNull(command);
        }

        [Test]
        [IgnoreAttribute("This won't work because we have a default command used by the other tests.")]
        public void Route_should_return_null_if_args_is_null()
        {
            // arrange
            string[] testArgs = null;

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNull(command);
        }

        [Test]
        [IgnoreAttribute("This won't work because we have a default command used by the other tests.")]
        public void Route_should_return_null_if_args_is_empty()
        {
            // arrange
            var testArgs = new string[] { };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNull(command);
        }
    }
}
