using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class MefCommandLocatorTestsWithBuiltinCommands
    {
        private MefCommandLocator _locator;

        [SetUp]
        public void Setup()
        {
            _locator = new MefCommandLocator(null);
        }

        public void GetCommand_should_return_HelpCommand(string commandName)
        {
            // arrange
            // act
            var command = _locator.GetCommandByName("help");

            // assert
            Assert.IsInstanceOf<MagicalHelpCommand>(command);
        }
    }
}