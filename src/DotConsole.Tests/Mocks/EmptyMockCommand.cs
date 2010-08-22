using System;

namespace DotConsole.Tests.Mocks
{
    public class EmptyMockCommand : ICommand
    {
        public string CommandName
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}