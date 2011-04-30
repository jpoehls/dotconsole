using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using DotConsole.Tests.Commands;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class MefCommandLocatorTestsWithTestCommands
    {
        private MefCommandLocator _locator;

        [SetUp]
        public void Setup()
        {
            _locator = new MefCommandLocator(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
        }

        [Test]
        public void GetDefaultCommand_should_return_TestHelpCommand()
        {
            // arrange
            // act
            var command = _locator.GetDefaultCommand();

            // assert
            Assert.IsInstanceOf<TestHelpCommand>(command);
        }

        public void GetCommand_should_return_TestHelpCommand(string commandName)
        {
            // arrange
            // act
            var command = _locator.GetCommandByName("help");

            // assert
            Assert.IsInstanceOf<TestHelpCommand>(command);
        }
    }
}