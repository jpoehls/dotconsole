using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotConsole.Tests.Mocks;
using Xunit;

namespace DotConsole.Tests
{
    public class CommandArgumentsUnitTests
    {
        [Fact]
        public void Parse_should_override_anonymous_arguments_with_named_arguments()
        {
            //  arrange
            ArgumentSet args = ArgumentSet.Parse(new[] {"joshua", "2", "-connection", "david", "-v", "123"});
            var opts = new MockCommandArgs();

            //  act
            opts.Parse(args);

            //  assert
            Assert.Equal("david", opts.Connection);
            Assert.Equal(123, opts.TargetVersion);
        }

        [Fact]
        public void Parse_should_set_property_values_by_position_of_anonymous_arguments()
        {
            //  arrange
            ArgumentSet args = ArgumentSet.Parse(new[] {"joshua", "2"});
            var opts = new MockCommandArgs();

            //  act
            opts.Parse(args);

            //  assert
            Assert.Equal("joshua", opts.Connection);
            Assert.Equal(2, opts.TargetVersion);
        }

        [Fact]
        public void Parse_should_set_property_values_to_matching_named_arguments()
        {
            //  arrange
            ArgumentSet args = ArgumentSet.Parse(new[] {"-connection", "joshua", "-v", "123"});
            var opts = new MockCommandArgs();

            //  act
            opts.Parse(args);

            //  assert
            Assert.Equal("joshua", opts.Connection);
            Assert.Equal(123, opts.TargetVersion);
        }

        [Fact]
        public void Parse_should_validate_properties()
        {
            //  arrange
            ArgumentSet args = ArgumentSet.Parse(new[] {"-v", "1"});
            var opts = new MockCommandArgs();

            //  act
            opts.Parse(args);

            //  assert
            Assert.False(opts.IsValid);
        }

        [Fact]
        public void Parse_should_validate_arguments_and_add_error_messages_to_collection()
        {
            //  arrange
            ArgumentSet args = ArgumentSet.Parse(new[] {"-v", "0"});
            var opts = new MockCommandArgs();

            //  act
            opts.Parse(args);

            //  assert
            Assert.Equal(2, opts.Errors.Count());
            Assert.Equal("Connection is required", opts.Errors.First());
            Assert.Equal("Target version must be between 1 and 5", opts.Errors.Last());
        }

        [Fact]
        public void GetArgumentProperties_should_return_all_properties_with_an_ArgumentAttribute()
        {
            //  act
            Dictionary<PropertyInfo, ArgumentAttribute> props =
                CommandArguments.GetArgumentProperties(typeof (MockCommandArgs));

            //  assert
            Assert.Equal(2, props.Count);
        }

        [Fact]
        public void GetArgumentProperties_should_return_argument_properties_ordered_by_their_Position_value()
        {
            //  act
            Dictionary<PropertyInfo, ArgumentAttribute> props =
                CommandArguments.GetArgumentProperties(typeof (MockCommandArgs));

            //  assert
            Assert.Equal(1, props.First().Value.Position);
            Assert.Equal(2, props.Skip(1).First().Value.Position);
        }
    }
}