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
        private readonly string[] _flagPrefixes;
        private readonly string[] _keySeparators;
        private readonly string[] _optionPrefixes;
        private readonly string[] _valueDelimiters;
        private readonly string[] _valueSeparators;

        public GnuPosixConventions()
        {
            _flagPrefixes = new[] {"-"};
            _optionPrefixes = new[] {"--"};
            _valueSeparators = new[] {"="};
            _keySeparators = new[] {":"};
            _valueDelimiters = new[] {",", " "};
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

        public IEnumerable<string> FlagPrefixes
        {
            get { return _flagPrefixes; }
        }

        public IEnumerable<string> OptionPrefixes
        {
            get { return _optionPrefixes; }
        }

        public IEnumerable<string> ValueSeparators
        {
            get { return _valueSeparators; }
        }

        public IEnumerable<string> KeySeparators
        {
            get { return _keySeparators; }
        }

        public IEnumerable<string> ValueDelimiters
        {
            get { return _valueDelimiters; }
        }

        #endregion
    }
}