using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace DotConsole
{
    [InheritedExport(typeof(ICommand))]
    public interface ICommand
    {
        IEnumerable<string> CommandNames { get; }
        void Execute();
    }
}