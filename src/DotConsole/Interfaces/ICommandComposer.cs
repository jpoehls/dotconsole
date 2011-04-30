using System;
using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandComposer
    {
        void ComposeParameters(ICommand command, IEnumerable<string> args);
    }
}