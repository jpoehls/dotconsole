namespace DotConsole
{
    public static class Commander
    {
        static Commander()
        {
            ArgumentParser = new ConventionBasedArgumentParser(ArgumentConventions.WindowsConventions);
            CommandRouter = Router.ArgumentBasedRouter;
        }

        /// <summary>
        /// Gets or sets the command router to use.
        /// </summary>
        public static IRouter CommandRouter { get; set; }

        /// <summary>
        /// Gets or sets the argument parser that should be used.
        /// </summary>
        public static IArgumentParser ArgumentParser { get; set; }

        /// <summary>
        /// This will parse the command line arguments,
        /// determine which command to run and run it
        /// with the correct arguments.
        /// </summary>
        public static void Run()
        {
            //  1. parse command line args

            //  2. get command to run
            //      a. first look for command name in args
            //      b. if command name specified doesn't exist, error
            //      c. if command exists, get it
            //      d. if no command specified, use default command
            //      e. if no defualt command, error

            //  3. look for public properties of types that implement ICommandArguments
            //     on the ICommand type

            //  4. set all the public properties on the ICommandArguments types
            //     using values from the parsed arguments

            //  5. validate the ICommandArguments types
            //      a. if validation fails, error

            //  6. run the command
        }
    }
}