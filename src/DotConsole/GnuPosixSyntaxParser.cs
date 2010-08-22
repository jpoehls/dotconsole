using System.Collections.Generic;

namespace DotConsole
{
    /// <summary>
    /// Parses command line arguments using the conventional GNU/POSIX syntax.
    /// </summary>
    public class GnuPosixSyntaxParser : ArgumentParser
    {
        /*
         * THESE ARE ALL EQUIVELANT
         * <yourscript> --file=outfile -q
         * <yourscript> -f outfile --quiet
         * <yourscript> --quiet --file outfile
         * <yourscript> -q -foutfile
         * <yourscript> -qfoutfile
         * 
         * -f flags are case-sensitive
         * 
         * dictionary options are supported like ( --file:key=value )
         *                                  or   ( -f:key=value )
         */
        //http://docs.python.org/library/optparse.html
        public override TArgs Parse<TArgs>(IEnumerable<string> args)
        {
            var t = new TArgs();
            var props = GetProperties<TArgs>();

            return t;
        }
    }
}