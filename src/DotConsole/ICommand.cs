using System.ComponentModel.Composition;

namespace DotConsole
{
    [InheritedExport(typeof(ICommand))]
    public interface ICommand
    {
        void Execute();
    }
}