using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using DotConsole.Tests.StubCommands;
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