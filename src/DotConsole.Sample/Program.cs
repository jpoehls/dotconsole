using System;
using System.Linq;

namespace DotConsole.Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // you have to tell the arg parser what you expect
            // you can do that thru a fluent api


            //  todo: optionally, set a default implicit command
            //        this will be used if no command is specified in the args
            //Commander.DefaultCommand = XYZ;

            //  this will parse the args, determine the command to run and run it
            Commander.Run();

            //  todo: optionally, register assemblies that contain commands here (add them to the aggregate catalog)
            //        so that DotConsole knows where to look
            //        DotConsole will look in the executing assembly by default
        }
    }
}