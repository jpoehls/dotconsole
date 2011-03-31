using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace DotConsole
{
    [InheritedExport("Commands", typeof(ICommand))]
    public interface ICommand //where TArgs : class
    {
        /// <summary>
        /// The name of the command that is typed as a command line argument.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// The help text description shown for the command.
        /// </summary>
        string Description { get; }

//        /// <summary>
//        /// Arguments of the command.
//        /// </summary>
//        TArgs Arguments { get; }

        void Run();
    }
}