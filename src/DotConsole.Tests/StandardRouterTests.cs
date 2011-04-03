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
        public void Route_should_return_command_with_a_name_matching_the_first_arg()
        {
            // arrange
            var testArgs = new[] { "help" };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNotNull(command, "command is null - routing failed");
            Assert.IsInstanceOf(typeof(HelpCommand), command, "wrong command returned");
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
            Assert.IsInstanceOf(typeof(HelpCommand), command, "wrong command returned");
        }

        [Test]
        public void Route_should_return_null_if_no_command_is_found()
        {
            // arrange
            var testArgs = new[] { "badcommand" };

            // act
            var command = _router.Route(testArgs);

            // assert
            Assert.IsNull(command);
        }

        [Test]
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
