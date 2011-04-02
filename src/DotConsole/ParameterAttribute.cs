﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute() : this(null) { }

        public ParameterAttribute(params string[] names)
        {
            Names = names ?? Enumerable.Empty<string>();
            Position = -1;
        }

        public IEnumerable<string> Names { get; set; }

        public int Position { get; set; }

        public bool IsCatchAll { get; set; }
    }
}