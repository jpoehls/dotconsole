using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace DotConsole
{
    /// <summary>
    /// Provides the meta data for a console command.
    /// This attribute should only be used on classes that implement <see cref="ICommand"/>.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandAttribute : ExportAttribute, ICommandMetadata
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandAttribute"/>
        /// using the decorated class's name as the command name.
        /// </summary>
        public CommandAttribute()
            : base(typeof(ICommand)) { }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="name">Name of the command as it will be entered at the command line.</param>
        public CommandAttribute(string name)
            : this()
        {
            _name = name;
        }

        private readonly string _name;

        /// <summary>
        /// Gets the name of the command as it will be entered at the command line.
        /// </summary>
        public string Name { get { return _name; } }
    }
}