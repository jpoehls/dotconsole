using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class MefCommandLocatorTests
    {
        private MefCommandLocator _locator;

        [SetUp]
        public void Setup()
        {
            _locator = new MefCommandLocator(null);
        }

        [Test]
        public void GetCommand_should_return_null_for_Help_command_before_it_is_registered()
        {
            // arrange
            // act
            var command = _locator.GetCommandByName(ReservedCommandNames.Help);

            // assert
            Assert.IsNull(command);
        }

        [Test]
        public void GetCommand_should_return_Help_command_after_it_is_registered_with_RegisterCommand()
        {
            // arrange
            _locator.RegisterCommand<MagicalHelpCommand>();

            // act
            var command = _locator.GetCommandByName(ReservedCommandNames.Help);

            // assert
            Assert.IsInstanceOf<MagicalHelpCommand>(command);
        }
    }
}