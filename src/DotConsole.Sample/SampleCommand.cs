using System;
using System.Linq;

namespace DotConsole.Sample
{
    internal class SampleCommand : CommandBase<SampleCommandArgs>
    {
        /// <summary>
        /// The name of the command that is typed as a command line argument.
        /// </summary>
        public override string CommandName
        {
            get { return "sample"; }
        }

        /// <summary>
        /// The help text information for the command.
        /// </summary>
        public override string Description
        {
            get { return "Does something."; }
        }

        protected override void Run(SampleCommandArgs args)
        {
            //  do something
        }
    }
}