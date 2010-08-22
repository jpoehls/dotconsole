using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace DotConsole
{
    [InheritedExport("Commands", typeof(ICommand))]
    public interface ICommand
    {
        /// <summary>
        /// The name of the command that is typed as a command line argument.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// The help text description shown for the command.
        /// </summary>
        string Description { get; }

        void Run();
    }
}