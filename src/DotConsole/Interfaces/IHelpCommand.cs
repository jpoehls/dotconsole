using System.Collections.Generic;

namespace DotConsole
{
    internal interface IHelpCommand : ICommand
    {
        IEnumerable<string> ErrorMessages { get; set; }
    }
}