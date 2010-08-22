using System;
using System.Collections.Generic;

namespace DotConsole
{
    public class UniversalSyntaxParser : ArgumentParser
    {
        //  support a mix match of both GNU/POSIX conventions and Windows conventions

        public override TArgs Parse<TArgs>(IEnumerable<string> args)
        {
            throw new NotImplementedException();
        }
    }
}