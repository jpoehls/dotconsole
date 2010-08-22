using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace DotConsole
{
    [InheritedExport("Commands", typeof (ICommand))]
    public interface ICommand
    {
        string CommandName { get; }
        string Description { get; }

        void Run(IArguments args);
        IArguments CreateArguments();
        Type GetArgumentsType();
    }
}