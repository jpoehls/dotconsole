using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DotConsole.Tests
{
    public class ArgumentBasedRouterTests
    {
        [Fact]
        public void FilterArguments_should_remove_the_first_argument()
        {
            //  arrange
            var router = new ArgumentBasedRouter();
            var originalArgs = new[] {"first", "second", "third"};

            //  act
            IEnumerable<string> filteredArgs = router.FilterArguments(originalArgs);

            //  assert
            Assert.Equal(originalArgs.Skip(1), filteredArgs);
        }
    }
}