using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute()
            : this(null)
        {
            Position = -1;
        }

        public ParameterAttribute(params string[] names)
        {
            Names = names ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> Names { get; set; }

        public int Position { get; set; }
    }
}