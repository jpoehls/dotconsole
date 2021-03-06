﻿using System;
using System.Collections.Generic;
using System.Linq;
using DotConsole.Tests.Mocks;
using Xunit;

namespace DotConsole.Tests
{
    public class StaticRouterTests
    {
        [Fact]
        public void GetCommand_should_return_new_instance_of_generic_type_given()
        {
            //  arrange
            var router = new StaticRouter<EmptyMockCommand>();

            //  act
            ICommand cmd = router.GetCommand(null);

            //  assert
            Assert.IsType(typeof (EmptyMockCommand), cmd);
        }
    }
}