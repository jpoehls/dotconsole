using System;
using System.Linq;

namespace DotConsole
{
    public class NamedArgumentsAttribute : Attribute
    {
        
    }

    public class AnonymousArgumentsAttribute : Attribute
    {
        
    }

    public class ArgumentAttribute : Attribute
    {
        public ArgumentAttribute(string name, string shortName, string desc)
        {
            Name = name;
            ShortName = shortName;
            Position = -1;
        }

        /// <summary>
        /// Long argument name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviated name that is friendly for the command line.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Used in the syntax help text as the placeholder for the value.
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// If the argument is not passed in with a name then it will
        /// be matched based on its position in the array of unnamed arguments.
        /// </summary>
        public int Position { get; set; }

        public string Description { get; set; }
    }
}