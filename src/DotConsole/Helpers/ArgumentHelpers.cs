using System.Collections.Generic;
using System.Linq;

namespace DotConsole.Helpers
{
    internal static class ArgumentHelpers
    {
        public static IEnumerable<string> GetNamedArgValues(this IEnumerable<string> args, IEnumerable<string> possibleNames)
        {
            var values = new List<string>();
            for (int i = 0; i < args.Count(); i++)
            {
                var arg = args.ElementAt(i);
                if (arg.StartsWith("/") || arg.StartsWith("--") || arg.StartsWith("-"))
                {
                    var name = arg.TrimStart("/-".ToCharArray());
                    if (possibleNames.Contains(name))
                    {
                        // assume value is the next argument in the list
                        values.Add(args.ElementAt(i + 1));

                        // skip the next argument since we counted it as the value
                        //i = i + 1;
                        break;
                    }
                }
            }

            return values;
        }
    }
}