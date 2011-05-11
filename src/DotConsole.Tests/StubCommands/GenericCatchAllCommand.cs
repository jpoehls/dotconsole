using System;

namespace DotConsole.Tests.StubCommands
{
    [Command("GenericCatchAllCommand")]
    public class GenericCatchAllCommand<TCatchAll> : ICommand
    {
        [Parameter(0, IsCatchAll = true)]
        public TCatchAll CatchAll { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}