﻿using DotConsole.Tests.StubCommands;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class StandardComposer_TestHelpCommand_Tests
    {
        private StandardComposer _composer;
        private TestHelpCommand _command;

        [SetUp]
        public void Setup()
        {
            _composer = new StandardComposer();
            _command = new TestHelpCommand();
        }

        [Test]
        public void ComposeParameters_should_set_positional_parameters()
        {
            // arrange
            var testArgs = new[] { "some topic", "low verbosity" };

            // act
            _composer.ComposeParameters(_command, testArgs);

            // assert
            Assert.AreEqual("some topic", _command.TopicName);
            Assert.AreEqual("low verbosity", _command.Verbosity);
        }
    }
}