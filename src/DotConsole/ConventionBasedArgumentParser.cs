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

            string name = null;
            int i = 0;
            while (i < args.Length)
            {
                if (IsFlag(args[i]))
                {
                    set.Flags.AddRange(GetFlags(args[i]));
                }

                if (HasValue(args, i))
                {
                    //ParseValues(args, i, set);
                }


                bool isName = IsName(args[i]);
                bool added = false;
                if (name != null && !isName)
                {
                    //set._namedArgs.Add(name, args[i]);
                    added = true;
                }
                else if (name != null)
                {
                    //set._namedArgs.Add(name, null);
                    added = true;
                }

                if (added)
                {
                    name = null;
                }

                if (isName)
                {
                    name = GetName(args[i]);
                }
                else if (!added)
                {
                    //set._anonymousArgs.Add(args[i]);
                }
            }

            if (name != null)
            {
                //set._namedArgs.Add(name, null);
            }

            return set;
        }

        private bool HasValue(string[] args, int i)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<char> GetFlags(string arg)
        {
            foreach (var prefix in _conventions.FlagPrefixes)
            {
                if (arg.StartsWith(prefix) && arg.Length > prefix.Length)
                {
                    return arg.Substring(prefix.Length);
                }
            }

            return null;
        }

        private bool IsFlag(string arg)
        {
            if (_conventions.FlagPrefixes.Any(prefix => arg.StartsWith(prefix) && arg.Length > prefix.Length))
            {
                return true;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Returns True/False whether the given
        /// argument is a name or not.
        /// </summary>
        private static bool IsName(string arg)
        {
            //            foreach (string prefix in NamePrefixes.Split('|'))
            //            {
            //                if (arg.StartsWith(prefix) && arg.Length > prefix.Length)
            //                {
            //                    return true;
            //                }
            //            }

            return false;
        }

        /// <summary>
        /// Returns the name without the prefix.
        /// </summary>
        private static string GetName(string arg)
        {
            //            foreach (string prefix in NamePrefixes.Split('|'))
            //            {
            //                if (arg.StartsWith(prefix) && arg.Length > prefix.Length)
            //                {
            //                    return arg.Substring(prefix.Length);
            //                }
            //            }

            return null;
        }
    }
}