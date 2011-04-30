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

        [Test]
        public void GetDefaultCommand_should_return_HelpCommand()
        {
            // arrange
            // act
            var command = _locator.GetDefaultCommand();

            // assert
            Assert.IsInstanceOf<MagicalHelpCommand>(command);
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