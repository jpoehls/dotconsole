using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    public class ConventionBasedArgumentParser : IArgumentParser
    {
        private readonly IArgumentConventions _conventions;

        public ConventionBasedArgumentParser(IArgumentConventions conventions)
        {
            _conventions = conventions;
        }

        #region IArgumentParser Members

        /// <summary>
        /// Parses a list of command line arguments into
        /// an argument set;
        /// </summary>
        public ArgumentSet Parse(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            var set = new ArgumentSet();

            int idx = 0;
            while (idx < args.Length)
            {
                if (_conventions.IsNamed(args[idx]))
                {
                    var name = _conventions.GetName(args[idx]);
                    var value = _conventions.GetValue(args[idx]);
                    set.NamedArguments.Add(name, value);
                }
                else
                {
                    set.PositionalArguments.Add(args[idx]);
                }
                idx++;
            }

            return set;
        }

        #endregion
    }
}