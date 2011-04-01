using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    public class StandardComposer : ICommandComposer
    {
        public virtual void ComposeParameters(ICommand command, IEnumerable<string> args)
        {
            if (command == null || args == null || args.Count() == 0)
                return;

            var parameterProps = command.GetParameters();
            foreach (var prop in parameterProps.Keys)
            {
                var paramInfo = parameterProps[prop];

                if (paramInfo.Names.Count() > 0)
                {
                    var value = FindValueByName(args, paramInfo.Names);
                    if (value != null)
                    {
                        command.SetParameterValue(prop, value);
                        continue;
                    }
                }

                if (paramInfo.Position >= 0)
                {
                    command.SetParameterValue(prop, args.ElementAtOrDefault(paramInfo.Position - 1));
                }
            }
        }

        private static string FindValueByName(IEnumerable<string> args, IEnumerable<string> possibleNames)
        {
            string value = null;
            for (int i = 0; i < args.Count(); i++)
            {
                var arg = args.ElementAt(i);
                if (arg.StartsWith("/") || arg.StartsWith("--") || arg.StartsWith("-"))
                {
                    var name = arg.TrimStart("/-".ToCharArray());
                    if (possibleNames.Contains(name))
                    {
                        // assume value is the next argument in the list
                        value = args.ElementAt(i + 1);

                        // skip the next argument since we counted it as the value
                        //i = i + 1;
                        break;
                    }
                }
            }

            return value;
        }
    }
}