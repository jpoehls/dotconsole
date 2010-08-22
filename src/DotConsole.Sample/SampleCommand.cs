using System;
using System.Linq;

namespace DotConsole.Sample
{
    public class SampleCommand : ICommand
    {
        public string CommandName
        {
            get { return "sample"; }
        }

        public string Description
        {
            get { return "Does something."; }
        }

        //  DotConsole will recognize this and assign
        //  this property value with the correct args
        public SampleCommandArgs Arguments { get; set; }

        public void Run()
        {
            //  do something
        }
    }
}