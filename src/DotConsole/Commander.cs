using System;
using System.Linq;

namespace DotConsole
{
    public class Commander
    {
        /// <summary>
        /// Gets or sets the default (implicit) command to run
        /// if no command is specified in the arguments.
        /// </summary>
        public static ICommand DefaultCommand { get; set; }

        /// <summary>
        /// This will parse the command line arguments,
        /// determine which command to run and run it
        /// with the correct arguments.
        /// </summary>
        public static void Run()
        {
        }
    }
}