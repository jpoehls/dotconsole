using System;
using System.Collections.Generic;

namespace DotConsole
{
    public class WindowsSyntaxParser : ArgumentParser
    {
        //  recognize / or - before option name
        //  /f (or -f) flags are case-insensitive
        //  spaces between option name and its value (ex. ping -w 100 -k host_list)
        //  or colons between option name and its value (ex. ping -w:100 -k:host_list)
        //  dictionary options are supported like ( /optionname:key=value )
        public override TArgs Parse<TArgs>(IEnumerable<string> args)
        {
            throw new NotImplementedException();
        }
    }
}