using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConsole
{
    /// <summary>
    /// Defines the conventions for GNU/POSIX command line arguments.
    /// </summary>
    public class GnuPosixConventions : IArgumentConventions
    {
        public GnuPosixConventions()
        {
//            _flagPrefixes = new[] { "-" };
//            _optionPrefixes = new[] { "--" };
//            _valueSeparators = new[] { "=" };
//            _keySeparators = new[] { ":" };
//            _valueDelimiters = new[] { ",", " " };
        }

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
         *                                  
         * list options are supported like ( --file=file1,file2,file3 )
         *                              or ( --file=file1 file2 file3 )
         */
        //http://docs.python.org/library/optparse.html
        //http://www.gnu.org/software/libc/manual/html_node/Argument-Syntax.html

        #region IArgumentConventions Members

        public bool IsNamed(string arg)
        {
            if (arg.StartsWith("-") && arg.Length > 1)
            {
                return true;
            }
            if (arg.StartsWith("--") && arg.Length > 2)
            {
                return true;
            }
            return false;
        }

        public string GetName(string arg)
        {
            if (arg.StartsWith("-") && arg.Length > 1)
            {
                return arg.Substring(1);
            }
            if (arg.StartsWith("--") && arg.Length > 2)
            {
                return arg.Substring(2);
            }
            return null;
        }

        public IArgumentValue GetValue(string arg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}