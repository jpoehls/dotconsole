using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandValidator
    {
        IEnumerable<string> ErrorMessages { get; }

        bool ValidateParameters(ICommand command);
    }
}