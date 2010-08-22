using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    public interface IArguments
    {
        bool IsValid { get; }
        IEnumerable<string> Errors { get; }
        void Parse(ArgumentSet argumentSet);
    }
}