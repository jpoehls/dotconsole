using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DotConsole.Tests
{
    public class ArgumentSetUnitTests
    {
        [Fact]
        public void ContainsName_should_be_case_insensitive()
        {
            //  arrange
            var args = new[] {"-HELP", "migrate"};
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  actC:\code\dotconsole\src\DotConsole.Tests\_Old\ArgumentSetUnitTests.cs
            bool exists = set.ContainsName("help");

            //  assert
            Assert.True(exists);
        }

        [Fact]
        public void ContainsName_should_return_true_if_name_exists()
        {
            //  arrange
            var args = new[] {"-help", "migrate"};
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  act
            bool exists = set.ContainsName("help");

            //  assert
            Assert.True(exists);
        }

        [Fact]
        public void GetByName_should_return_value_of_argument_with_given_name()
        {
            //  arrange
            var args = new[] {"-help", "migrate"};
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  act
            string value = set.GetByName("help");

            //  assert
            const string expectedValue = "migrate";
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void GetByName_should_throw_KeyNotFoundException_if_name_doesnt_exist()
        {
            //  arrange
            var args = new[] {"blah"};
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  act, assert
            Assert.Throws<KeyNotFoundException>(() => set.GetByName("help"));
        }

        [Fact]
        public void Parse_should_find_anonymous_arguments_between_named_arguments()
        {
            //  arrange
            var args = new[] {"-help", "migrate", "joshua", "david", "-c", "connection"};

            //  act
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  assert
            Assert.Equal(2, set.NamedArgs.Count());
            Assert.Equal(2, set.AnonymousArgs.Count());
            Assert.Equal("joshua", set.AnonymousArgs.First());
            Assert.Equal("david", set.AnonymousArgs.Skip(1).First());
        }

        [Fact]
        public void Parse_should_find_named_arguments_with_no_value()
        {
            //  arrange
            var args = new[] {"-help", "-me"};

            //  act
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  assert
            Assert.Equal(2, set.NamedArgs.Count());
            Assert.Equal(null, set.GetByName("help"));
            Assert.Equal(null, set.GetByName("me"));
        }

        [Fact]
        public void Parse_should_match_dash_names()
        {
            //  arrange
            var args = new[] {"-help", "migrate"};

            //  act
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  assert
            Assert.True(set.ContainsName("help"));
        }

        [Fact]
        public void Parse_should_match_forward_slash_names()
        {
            //  arrange
            var args = new[] {"/help", "migrate"};

            //  act
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  assert
            Assert.True(set.ContainsName("help"));
        }

        [Fact]
        public void Parse_should_match_multiple_named_arguments_and_values()
        {
            //  arrange
            var args = new[] {"-help", "migrate", "-c", "connection"};

            //  act
            OldArgumentSet set = OldArgumentSet.Parse(args);

            //  assert
            Assert.Equal(2, set.NamedArgs.Count());
            Assert.Equal("migrate", set.GetByName("help"));
            Assert.Equal("connection", set.GetByName("c"));
        }

        [Fact]
        public void Parse_should_throw_ArgumentNullException_if_args_param_is_null()
        {
            //  act, assert
            Assert.Throws<ArgumentNullException>(() => OldArgumentSet.Parse(null));
        }
    }
}