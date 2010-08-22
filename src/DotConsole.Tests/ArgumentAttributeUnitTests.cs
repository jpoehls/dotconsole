using System;
using System.Linq;
using Xunit;

namespace DotConsole.Tests
{
    public class ArgumentAttributeUnitTests
    {
        [Fact]
        public void Constructor_should_initialize_properties()
        {
            //  arrange
            const string name = "name";
            const string shortName = "short_name";
            const string desc = "description";

            //  act
            var attr = new ArgumentAttribute(name, shortName, desc);

            //  assert
            Assert.Equal(name, attr.Name);
            Assert.Equal(shortName, attr.ShortName);
            Assert.Equal(desc, attr.Description);
        }

        [Fact]
        public void Constructor_should_default_Position_to_max_int_value()
        {
            //  act
            var attr = new ArgumentAttribute(null, null, null);

            //  assert
            Assert.Equal(int.MaxValue, attr.Position);
        }
    }
}