using System.Collections.Generic;

namespace DotConsole
{
    public interface IArgumentConventions
    {
        IEnumerable<string> FlagPrefixes { get; }
        IEnumerable<string> OptionPrefixes { get; }
        IEnumerable<string> ValueSeparators { get; }
        IEnumerable<string> KeySeparators { get; }
        IEnumerable<string> ValueDelimiters { get; }
    }
}