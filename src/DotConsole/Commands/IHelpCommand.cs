using System.Collections.Generic;

namespace DotConsole.Commands
{
    internal interface IHelpCommand : ICommand
    {
        IEnumerable<string> ErrorMessages { get; set; }
    }
}