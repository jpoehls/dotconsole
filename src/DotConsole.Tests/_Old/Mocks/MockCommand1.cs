using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace DotConsole.Tests.Mocks
{
    [Export("Commands", typeof (ICommand))]
    internal class MockCommand1 : ICommand
    {
        public string CommandName
        {
            get { return "TestCommand"; }
        }

        public string Description
        {
            get { return "This is help text for MockCommand1."; }
        }

        public bool RunShouldThrowException { get; set; }

        public void Run()
        {
            if (RunShouldThrowException)
            {
                throw new ApplicationException("error!");
            }
        }
    }
}