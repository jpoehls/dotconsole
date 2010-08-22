using System;
using System.Linq;

namespace DotConsole
{
    public class CommandEventArgs<TArgs> : EventArgs
        where TArgs : CommandArguments
    {
        public CommandEventArgs(TArgs commandArguments)
        {
            CommandArguments = commandArguments;
        }

        public TArgs CommandArguments { get; private set; }
    }
}