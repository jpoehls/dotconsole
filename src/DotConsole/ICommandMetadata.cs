using System.Collections.Generic;

namespace DotConsole
{
    public interface ICommandMetadata
    {
        IEnumerable<string> Names { get; }
        bool IsDefault { get; }
    }
}