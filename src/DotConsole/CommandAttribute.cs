using System;
using System.ComponentModel.Composition;

namespace DotConsole
{
    /// <summary>
    /// Provides the meta data for a console command.
    /// This attribute should only be used on classes that implement <see cref="ICommand"/>.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : ExportAttribute, ICommandMetadata
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="name">Name of the command as it will be entered at the command line.</param>
        public CommandAttribute(string name)
            : base(typeof(ICommand))
        {
            Name = name;
        }

        /// <summary>
        /// Gets/Sets the name of the command as it will be entered at the command line.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets/Sets whether this is the default command if
        /// no specific command is specified in the command line arguments.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}