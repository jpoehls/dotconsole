﻿using System;
using System.Linq;
using Xunit;

namespace DotConsole.Tests
{
    public class ConventionBasedArgumentParserTests
    {
        private readonly ConventionBasedArgumentParser _parser;

        public ConventionBasedArgumentParserTests()
        {
            _parser = new ConventionBasedArgumentParser(ArgumentConventions.GnuPosixConventions);
        }

        //  commandName -abcd --file out.txt --include 1,2,3 4 -e -f -g

        [Fact]
        public void Parse_should_get_flags_pushed_together()
        {
            //  arrange
//            var args = new[] { "-abcd" };
//
            //  act
//            var set = _parser.Parse(args);
//
            //  assert
//            Assert.True(set.Flags.Contains('a'));
//            Assert.True(set.Flags.Contains('b'));
//            Assert.True(set.Flags.Contains('c'));
//            Assert.True(set.Flags.Contains('d'));
        }
    }
}