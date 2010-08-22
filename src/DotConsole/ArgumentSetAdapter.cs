using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public class ArgumentSetAdapter
    {
        /// <summary>
        /// Populates a new ICommandArguments object with values
        /// from the given argument set;
        /// </summary>
        public TArgs PopulateObject<TArgs>(ArgumentSet argumentSet)
            where TArgs : ICommandArguments, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the properties of a type that should be
        /// populated with parsed argument values.
        /// </summary>
        protected IEnumerable<PropertyInfo> GetProperties<T>()
        {
            var type = typeof(T);

            var q = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite);

            return q;
        }
    }
}