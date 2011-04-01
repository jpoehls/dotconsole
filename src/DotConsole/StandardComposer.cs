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
                    var value = args.GetNamedArgValues(paramInfo.Names).FirstOrDefault();
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
    }
}