using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace DotConsole.Tests
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
     *                                  
     * list options are supported like ( --file=file1,file2,file3 )
     *                              or ( --file=file1 file2 file3 )
     */
    //http://docs.python.org/library/optparse.html
    //http://www.gnu.org/software/libc/manual/html_node/Argument-Syntax.html


    //  recognize / or - before option name
    //  /f (or -f) flags are case-insensitive
    //  spaces between option name and its value (ex. ping -w 100 -k host_list)
    //  or colons between option name and its value (ex. ping -w:100 -k:host_list)
    //  dictionary options are supported like ( /optionname:key=value )


    public class RegexArgParserTests
    {
        [Fact]
        public void TestCase1()
        {
            var args = new[]
                           {
                               "c:\\tools\\argtest.exe",
                               "--file=outfile",
                               "-q",
                               "--params:name=joshua",
                               "--params:age=24,dob=08-16-86",
                               "-abc=123",
                               "--params:gender=m; city=Dewey",
                               "--recipients=kristina,joshua;mark",
                               "positional 1",
                               "positional 2"
                           };

            var set = RegexArgParser.ParseToSet(args);

            Console.WriteLine(set);

            Assert.True(set.ContainsName("file"), "file arg not found");
            Assert.True(set.GetByName("file").Values.First() == "outfile", "file arg missing value");
            Assert.True(set.ContainsName("q"), "q arg not found");
        }

        [Fact]
        public void TestCase2()
        {
            var args = new[]
                           {
                               "c:\\tools\\argtest.exe",
                               "-f",
                               "outfile",
                               "--quiet"
                           };

            var parsed = RegexArgParser.ParseToSet(args);

            Assert.True(parsed.ContainsName("f"), "missing 'f'");
            Assert.True(parsed.GetByName("f").Values.First() == "outfile", "f's value is missing");
            Assert.True(parsed.ContainsName("quiet"), "missing 'quiet'");
        }

        [Fact]
        public void TestCase3()
        {
            var args = new[]
                           {
                               "c:\\tools\\argtest.exe",
                               "--quiet",
                               "--file",
                               "outfile"
                           };

            var parsed = RegexArgParser.ParseToSet(args);

            Assert.True(parsed.ContainsName("file"), "missing 'file'");
            Assert.True(parsed.GetByName("file").Values.First() == "outfile", "file's value is missing");
            Assert.True(parsed.ContainsName("quiet"), "missing 'quiet'");
        }

        [Fact]
        public void TestCase4()
        {
            var args = new[]
                           {
                               "c:\\tools\\argtest.exe",
                               "-qf",
                               "outfile"
                           };

            var parsed = RegexArgParser.ParseToSet(args);

            Assert.True(parsed.ContainsName("f"), "missing 'f'");
            Assert.True(parsed.GetByName("f").Values.First() == "outfile", "f's value is missing");
            Assert.True(parsed.ContainsName("q"), "missing 'q'");
        }

        [Fact]
        public void TestCase5()
        {
            var args = new[]
                           {
                               "c:\\tools\\argtest.exe",
                               "--file:outfile",
                               "--quiet"
                           };

            var parsed = RegexArgParser.ParseToSet(args);

            Assert.True(parsed.ContainsName("file"), "missing 'file'");
            Assert.True(parsed.GetByName("file").Values.First() == "outfile", "file's value is missing");
            Assert.True(parsed.ContainsName("quiet"), "missing 'quiet'");
        }

        [Fact]
        public void TestCase6()
        {
            var args = new[]
                           {
                               "c:\\tools\\argtest.exe",
                               "-qf:outfile"
                           };

            var parsed = RegexArgParser.ParseToSet(args);

            Assert.True(parsed.ContainsName("f"), "missing 'f'");
            Assert.True(parsed.GetByName("f").Values.First() == "outfile", "f's value is missing");
            Assert.True(parsed.ContainsName("q"), "missing 'q'");
        }
    }

    public class ArgSet
    {
        public ArgSet(string executableName, IEnumerable<RegexArgParser.Arg> namedArgs, IEnumerable<string> positionalArgs)
        {
            ExecutableName = executableName;
            NamedArgs = namedArgs;
            PositionalArgs = positionalArgs;
        }

        public string ExecutableName { get; private set; }
        public IEnumerable<RegexArgParser.Arg> NamedArgs { get; private set; }
        public IEnumerable<string> PositionalArgs { get; private set; }

        public bool ContainsName(string name)
        {
            var c = NamedArgs.Count(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            return c > 0;
        }

        public RegexArgParser.Arg GetByName(string name)
        {
            return
                NamedArgs.Where(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Executable Name:");
            sb.AppendFormat("  {0}{1}{1}", ExecutableName, Environment.NewLine);

            sb.AppendLine("Named Args:");
            foreach (var pair in NamedArgs)
            {
                sb.AppendFormat("  * {0} => {1}{2}", pair.Name, pair.Values, Environment.NewLine);
            }
            sb.AppendLine();
            sb.AppendLine("Positional Args:");
            foreach (var arg in PositionalArgs)
            {
                sb.AppendFormat("  * {0}{1}", arg, Environment.NewLine);
            }

            return sb.ToString();
        }
    }

    public class RegexArgParser
    {
        public class Arg
        {
            public string Name { get; set; }
            public IEnumerable<string> Values { get; set; }
            public IEnumerable<KeyValuePair<string, string>> KeyedValues { get; set; }
        }

        public static ArgSet ParseToSet(IEnumerable<string> args, params char[] expectedFlags)
        {
            var namedArgs = new List<Arg>();
            var positionalArgs = new List<string>();

            foreach (var arg in args.Skip(1))
            {
                // match arguments prefixed by -, -- or /
                // allow the value to be separated from the name by a : or =
                var m = Regex.Match(arg, @"^(?<marker>-{1,2}|/)(?<name>[\w\d]+)([:=](?<value>.+))?$");
                if (m.Success)
                {
                    string argName = m.Groups["name"].Value;

                    string marker = m.Groups["marker"].Value;
                    if (marker == "-")
                    {
                        // marker is for flags, make the arg name the last flag in the list
                        // and add the rest of the flags to the named args array
                        if (argName.Length > 1)
                        {
                            char[] flags = argName.ToCharArray(0, argName.Length - 1);
                            foreach (var flag in flags)
                            {
                                if (namedArgs.Count(a => a.Name == flag.ToString()) == 0)
                                {
                                    namedArgs.Add(new Arg { Name = flag.ToString() });
                                }
                            }

                            argName = argName.Last().ToString();
                        }
                    }

                    var argValues = new List<string>();
                    var argKeyedValues = new List<KeyValuePair<string, string>>();

                    // if we have already parsed an arg with this name
                    // then add those parsed values to our collections
                    // and remove the existing arg from the list (we'll add it back later)
                    var existingArg = namedArgs.SingleOrDefault(a => a.Name == argName);
                    if (existingArg != null)
                    {
                        namedArgs.Remove(existingArg);
                        argValues.AddRange(existingArg.Values);
                        argKeyedValues.AddRange(existingArg.KeyedValues);
                    }

                    //string key = m.Groups["key"].Value;
                    bool hasValue = m.Groups["value"].Success;

                    if (hasValue)
                    {
                        string fullValue = m.Groups["value"].Value;

                        // split on a , or ; surrounded by optional whitespace
                        // unless it is escaped by a backslash \
                        string[] valueSplit = Regex.Split(fullValue, @"\s*(?<!\\)[;,]\s*");

                        foreach (var v in valueSplit)
                        {
                            // split on equals sign = surrounded by optional whitespace
                            // unless it is escaped by a backslash \
                            string[] keyValueSplit = Regex.Split(v, @"\s*(?<!\\)=\s*");
                            if (keyValueSplit.Length > 1)
                            {
                                string key = keyValueSplit[0];
                                string value = keyValueSplit[1];
                                argKeyedValues.Add(new KeyValuePair<string, string>(key, value));
                            }

                            argValues.Add(fullValue);
                        }
                    }

                    namedArgs.Add(new Arg
                                      {
                                          Name = argName,
                                          Values = argValues,
                                          KeyedValues = argKeyedValues
                                      });
                }
                else
                {
                    // it isn't a named argument, add it to the positional arg list
                    positionalArgs.Add(arg);
                }
            }

            var set = new ArgSet(args.First(), namedArgs, positionalArgs);
            return set;
        }

        public static T ParseToObject<T>(string[] args, T target)
        {
            //  TODO: populate object's properties with the values of their matching named arguments
            //  TODO: if the object has a 'PositionalArgs' property that is a collection, populate it with the positional arguments
            return target;
        }
    }
}
