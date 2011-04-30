using DotConsole.Tests.Commands;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class StandardComposer_MigrateCommand_Tests
    {
        private StandardComposer _composer;
        private MigrateCommand _command;

        [SetUp]
        public void Setup()
        {
            _composer = new StandardComposer();
            _command = new MigrateCommand();
        }

        [TestCase("/c", "some connection", Description = "Flag used")]
        [TestCase("some connection", null, Description = "Position used")] // the null is to make NUnit happy
        public void ComposeParameters_should_parse_Connection_parameter_when_Version_args_are_missing(params string[] testArgs)
        {
            // act
            _composer.ComposeParameters(_command, testArgs);

            // assert
            Assert.AreEqual("some connection", _command.Connection);
        }

        [TestCase("some connection", "101", Description = "No names")]
        [TestCase("/c", "some connection", "101", Description = "/c named")]
        public void ComposeParameters_should_set_named_parameters_by_position_when_names_are_omitted(params string[] testArgs)
        {
            // act
            _composer.ComposeParameters(_command, testArgs);

            // assert
            Assert.AreEqual("some connection", _command.Connection);
            Assert.AreEqual(101, _command.TargetVersion);
        }

        [TestCase("/v", "101", "/c", "some connection")]
        public void ComposeParameters_should_set_named_parameters_regardless_of_position(params string[] testArgs)
        {
            // act
            _composer.ComposeParameters(_command, testArgs);

            // assert
            Assert.AreEqual("some connection", _command.Connection);
            Assert.AreEqual(101, _command.TargetVersion);
        }

        [TestCase("/connection", "some connection", "/version", "101", Description = "Using long names with / prefix")]
        [TestCase("--connection", "some connection", "--version", "101", Description = "Using long names with -- prefix")]
        [TestCase("/c", "some connection", "/v", "101", Description = "Using short names with / prefix")]
        public void ComposeParameters_should_set_args_with_various_styles(params string[] testArgs)
        {
            // act
            _composer.ComposeParameters(_command, testArgs);

            // assert
            Assert.AreEqual("some connection", _command.Connection);
            Assert.AreEqual(101, _command.TargetVersion);
        }
    }
}