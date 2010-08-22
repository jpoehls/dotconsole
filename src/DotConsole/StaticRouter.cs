using System.Collections.Generic;

namespace DotConsole
{
    /// <summary>
    /// Always uses the specified command.
    /// </summary>
    public class StaticRouter<TCommand> : IRouter
        where TCommand : ICommand, new()
    {
        public ICommand GetCommand(IEnumerable<string> args)
        {
            return new TCommand();
        }

        public IEnumerable<string> FilterArguments(IEnumerable<string> args)
        {
            return args;
        }
    }
}