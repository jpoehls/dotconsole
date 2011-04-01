using DotConsole.Tests.Commands;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class StandardComposerTests
    {
        private StandardComposer _composer;

        [SetUp]
        public void Setup()
        {
            _composer = new StandardComposer();
        }

        [Test]
        public void ComposeParameters_should_set_positional_parameters()
        {
            // arrange
            var helpCommand = new HelpCommand();
            var testArgs = new[] { "some topic", "low verbosity" };

            // act
            _composer.ComposeParameters(helpCommand, testArgs);

            // assert
            Assert.AreEqual("some topic", helpCommand.TopicName);
            Assert.AreEqual("low verbosity", helpCommand.Verbosity);
        }

        [Test]
        public void ComposeParameters_should_set_named_parameters_by_position_when_names_are_omitted()
        {
            // arrange
            var migrateCommand = new MigrateCommand();
            var testArgs = new[] { "some connection", "101" };

            // act
            _composer.ComposeParameters(migrateCommand, testArgs);

            // assert
            Assert.AreEqual("some connection", migrateCommand.Connection);
            Assert.AreEqual(101, migrateCommand.TargetVersion);
        }

        [Test]
        public void ComposeParameters_should_set_named_parameters_with_slashes_and_values_separated_by_spaces()
        {
            // arrange
            var migrateCommand = new MigrateCommand();
            var testArgs = new[] { "/connection", "some connection", "/version", "101" };

            // act
            _composer.ComposeParameters(migrateCommand, testArgs);

            // assert
            Assert.AreEqual("some connection", migrateCommand.Connection);
            Assert.AreEqual(101, migrateCommand.TargetVersion);
        }
    }
}