using System;
using System.ComponentModel.Composition;

namespace DotConsole
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : ExportAttribute, ICommandMetadata
    {
        public CommandAttribute() : this(null) { }

        public CommandAttribute(string name)
            : base(typeof(ICommand))
        {
            Name = name;
        }

        public string Name { get; private set; }

        public bool IsDefault { get; set; }
    }
}