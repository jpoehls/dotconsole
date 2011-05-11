using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using DotConsole.Routing;
using DotConsole.Tests.StubCommands;
using NUnit.Framework;

namespace DotConsole.Tests.Routing
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
        public void GetCommand_should_return_TestHelpCommand()
        {
            // arrange
            // act
            var command = _locator.GetCommandByName("testhelp");

            // assert
            Assert.IsInstanceOf<TestHelpCommand>(command);
        }
    }
}