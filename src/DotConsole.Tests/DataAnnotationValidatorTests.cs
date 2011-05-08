using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotConsole.Tests.Commands;
using NUnit.Framework;

namespace DotConsole.Tests
{
    [TestFixture]
    public class DataAnnotationValidatorTests
    {
        private DataAnnotationValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new DataAnnotationValidator();
        }

        [Test]
        public void ValidateParameters_should_return_false_if_validation_fails()
        {
            // arrange
            var cmd = new MigrateCommand();

            // act
            var valid = _validator.ValidateParameters(cmd);

            // assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void ValidateParameters_should_return_true_if_validation_succeeds()
        {
            // arrange
            var cmd = new MigrateCommand();
            cmd.Connection = "blah";

            // act
            var valid = _validator.ValidateParameters(cmd);

            // assert
            Assert.IsTrue(valid);
        }

        [Test]
        public void ErrorMessages_should_contain_validation_errors()
        {
            // arrange
            var cmd = new MigrateCommand();

            // act
            _validator.ValidateParameters(cmd);

            // assert
            Assert.AreEqual(1, _validator.ErrorMessages.Count());
        }

        [Test]
        public void ErrorMessages_should_be_empty_if_validation_succeeds()
        {
            // arrange
            var cmd = new MigrateCommand();
            cmd.Connection = "blah";

            // act
            _validator.ValidateParameters(cmd);

            // assert
            Assert.AreEqual(0, _validator.ErrorMessages.Count());
        }
    }
}
