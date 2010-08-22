using System;
using System.Collections.Generic;

namespace DotConsole
{
    /// <summary>
    /// Defines the conventions for Windows command line arguments.
    /// </summary>
    public class WindowsConventions : IArgumentConventions
    {
        private readonly string[] _flagPrefixes;
        private readonly string[] _keySeparators;
        private readonly string[] _optionPrefixes;
        private readonly string[] _valueDelimiters;
        private readonly string[] _valueSeparators;

        public WindowsConventions()
        {
            _flagPrefixes = new[] { "/", "-" };
            _optionPrefixes = new[] { "/", "-" };
            _valueSeparators = new[] { ":", "=" };
            _keySeparators = new[] { ":" };
            _valueDelimiters = new[] { ",", ";", " " };
        }

        //  recognize / or - before option name
        //  /f (or -f) flags are case-insensitive
        //  spaces between option name and its value (ex. ping -w 100 -k host_list)
        //  or colons between option name and its value (ex. ping -w:100 -k:host_list)
        //  dictionary options are supported like ( /optionname:key=value )

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