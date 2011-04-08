using System;

namespace DotConsole
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute() : this(null) { }

        public ParameterAttribute(string name)
        {
            Name = name;
            Position = -1;
        }

        public string Name { get; private set; }

        public char Flag { get; set; }

        public string MetaName { get; set; }

        public int Position { get; set; }

        public bool IsCatchAll { get; set; }
        
        public string GetMetaName()
        {
            if (string.IsNullOrWhiteSpace(MetaName))
                return Name;

            return MetaName;
        }
    }
}