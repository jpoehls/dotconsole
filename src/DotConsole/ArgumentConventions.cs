namespace DotConsole
{
    public static class ArgumentConventions
    {
        public static readonly GnuPosixConventions GnuPosixConventions = new GnuPosixConventions();
        public static readonly WindowsConventions WindowsConventions = new WindowsConventions();   
    }
}