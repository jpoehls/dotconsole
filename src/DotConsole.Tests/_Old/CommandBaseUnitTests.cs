using System;
using System.Linq;
using DotConsole.Tests.Mocks;
using Xunit;

namespace DotConsole.Tests
{
    public class CommandBaseUnitTests
    {
        [Fact]
        public void CreateArguments_should_return_instance_of_generic_TArgs_type()
        {
            //  arrange
            var cmd = new MockCommand1();

            //  act
            IArguments args = cmd.CreateArguments();

            //  assert
            Type expectedType = typeof (MockCommandArgs);
            Assert.True(expectedType.IsInstanceOfType(args));
        }

        [Fact]
        public void GetArgumentsType_should_return_generic_TArgs_type()
        {
            //  arrange
            var cmd = new MockCommand1();

            //  act
            Type argsType = cmd.GetArgumentsType();

            //  assert
            Type expectedType = typeof (MockCommandArgs);
            Assert.Equal(expectedType, argsType);
        }

        [Fact]
        public void Run_should_throw_ArgumentException_if_args_type_doesnt_match_generic_TArgs()
        {
            //  arrange
            var cmd = new MockCommand1();
            var args = (new Moq.Mock<CommandArguments>()).Object;

            //  act
            var ex = Assert.Throws<ArgumentException>(() => cmd.Run(args));

            //  assert
            Assert.Equal("args type doesn't match generic type\r\nParameter name: args", ex.Message);
        }

        [Fact]
        public void Run_should_throw_ArgumentNullException_if_given_null_args()
        {
            //  arrange
            var cmd = new MockCommand1();

            //  act
            Assert.Throws<ArgumentNullException>(() => cmd.Run(null));
        }

        [Fact]
        public void Run_should_throw_InvalidOperationException_if_args_are_not_valid()
        {
            //  arrange
            var cmd = new MockCommand1();
            ArgumentSet argSet = ArgumentSet.Parse(new[] {"blah"});
            var args = new MockCommandArgs();
            args.Parse(argSet);

            //  act, assert
            var ex = Assert.Throws<InvalidOperationException>(() => cmd.Run(args));
            Assert.Equal("Argument validation failed. Arguments are invalid.", ex.Message);
        }
    }
}