using System;
using System.Linq;

namespace DotConsole
{
    /// <summary>
    /// Always uses the specified command.
    /// </summary>
    public class StaticRouter<TCommand> : IRouter
        where TCommand : ICommand, new()
    {
        #region IRouter Members

        public ICommand GetCommand(ArgumentSet args)
        {
            return new TCommand();
        }

        #endregion
    }
}