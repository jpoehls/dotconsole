using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public abstract class ArgumentParser
    {
        public static readonly GnuPosixSyntaxParser GnuPosixSyntax = new GnuPosixSyntaxParser();
        public static readonly WindowsSyntaxParser WindowsSyntax = new WindowsSyntaxParser();
        public static readonly UniversalSyntaxParser UniversalSyntax = new UniversalSyntaxParser();

        /// <summary>
        /// Parses a list of command line arguments and
        /// returns a strongly typed class populated with
        /// their values.
        /// </summary>
        public abstract TArgs Parse<TArgs>(IEnumerable<string> args)
            where TArgs : ICommandArguments, new();

        /// <summary>
        /// Gets the properties of a type that should be
        /// populated with parsed argument values.
        /// </summary>
        protected IEnumerable<PropertyInfo> GetProperties<T>()
        {
            var type = typeof (T);

            var q = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite);

            return q;
        }
    }
}