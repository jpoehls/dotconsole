using System;

namespace DotConsole
{
    /// <summary>
    /// Provides the meta data for a console command parameter.
    /// This attribute should only be used on public properties of classes
    /// that implement <see cref="ICommand"/>.
    /// </summary>
    /// <remarks>
    /// A parameter is required to specify at least one of the following:
    /// a name, a flag, or a position.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        private string _metaName;

        private ParameterAttribute()
        {
            Position = -1;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterAttribute"/>.
        /// </summary>
        /// <param name="name">Name of the parameter as it will be entered at the command line.</param>
        public ParameterAttribute(string name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterAttribute"/>.
        /// </summary>
        /// <param name="flag">Flag character of the parameter as it will be entered at the command line.</param>
        public ParameterAttribute(char flag)
            : this()
        {
            Flag = flag;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterAttribute"/>.
        /// </summary>
        /// <param name="position">Position at which this parameter should be expected if it is not passed be <see cref="Name"/> or <see cref="Flag"/>.</param>
        public ParameterAttribute(int position)
            : this()
        {
            Position = position;
        }

        /// <summary>
        /// Gets/Sets the name of the parameter as it will be entered at the command line.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets the flag character of the parameter as it will be entered at the command line.
        /// </summary>
        public char Flag { get; set; }

        /// <summary>
        /// Gets/Sets the position at which this parameter should be expected
        /// if it is not passed by <see cref="Name"/> or <see cref="Flag"/>.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets/Sets the meta name to use when referring to the value
        /// of this parameter in the command's usage help output.
        /// </summary>
        public string MetaName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_metaName))
                {
                    return Name;
                }

                return _metaName;
            }
            set { _metaName = value; }
        }

        /// <summary>
        /// Gets/Sets whether this parameter should be treated as the catch-all
        /// for any unexpected parameters that are passed to the command.
        /// </summary>
        public bool IsCatchAll { get; set; }
    }
}