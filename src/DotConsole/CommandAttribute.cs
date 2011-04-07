using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace DotConsole
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : ExportAttribute, ICommandMetadata
    {
        public CommandAttribute() : this(null) { }

        public CommandAttribute(params string[] names)
            : base(typeof(ICommand))
        {
            Names = names ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> Names { get; private set; }

        public bool IsDefault { get; set; }
    }
}