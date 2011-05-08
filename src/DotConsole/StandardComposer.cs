using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public class StandardComposer : ICommandComposer
    {
        private class ParsableArg
        {
            public string Arg { get; set; }
            public bool Used { get; set; }
            public bool IsName { get; set; }
            public int Position { get; set; }
        }

        public StandardComposer()
        {
            IgnoreEnumCasing = true;
            ParameterNameComparer = StringComparer.OrdinalIgnoreCase;
        }

        // TODO: use the IgnoreEnumCasing property
        /// <summary>
        /// Gets/Sets whether to ignore casing when parsing enum values.
        /// By default this is True and casing is ignored.
        /// </summary>
        public bool IgnoreEnumCasing { get; set; }

        // TODO: use the ParameterNameComparer property
        /// <summary>
        /// <see ref="StringComparer" /> to use when matching parameter names.
        /// </summary>
        public StringComparer ParameterNameComparer { get; set; }

        // TODO: change this to a regex. names can only by ^[\w\d\-_]+$ -- but regex should support parsing any embedded value as well
        protected static readonly string[] ParameterNamePrefixes = new[] { "--", "/" };

        public virtual void ComposeParameters(ICommand command, IEnumerable<string> args)
        {
            if (command == null || args == null || args.Count() == 0)
                return;

            var parameterProps = command.GetParameters();
            var argList = args
                .Where(x => x != null)
                .Select(x => new ParsableArg { Used = false, Arg = x })
                .ToList();

            // keep track of which parameters we have processed (found potential values for)
            var processedParameters = new List<PropertyInfo>();

            #region - Assign named args -

            // for each of the command's parameters
            foreach (var prop in parameterProps.Keys)
            {
                var paramInfo = parameterProps[prop];

                // if this is a named parameter
                if (!string.IsNullOrEmpty(paramInfo.Name))
                {
                    // the look for matching named args
                    foreach (var arg in argList)
                    {
                        if (arg.Used)
                            continue;

                        if (IsMatchingNamedArg(arg.Arg, paramInfo.Name, paramInfo.Flag))
                        {
                            arg.Used = true;
                            arg.IsName = true;
                            processedParameters.Add(prop);

                            var embeddedValue = GetValueFromNamedArg(arg.Arg);

                            if (typeof(IList).IsAssignableFrom(prop.PropertyType))
                            {
                                var list = (IList)prop.GetValue(command, null);

                                // else if parameter is ILIST
                                //   then get value from arg (split on = or : then on , or ;)
                                //   if no value then take every arg after it as a value until we hit an arg that is a name (--|/)

                                // TODO: add multi-value splitting and add items to the list
                            }
                            // TODO: add support for dictionaries with any key and value type
                            else if (typeof(IDictionary<string, string>).IsAssignableFrom(prop.PropertyType))
                            {
                                var dictionary = (IDictionary<string, string>)prop.GetValue(command, null);

                                // else if parameter is IDICTIONARY
                                //   then get value from arg (split on = or :, then on , or ;, split again on = or : to get keys and values)
                                //   if no value then take every arg after it as a value (and do same splits) until we hit an arg that is a name (--|/)

                                // TODO: add key/value parsing and add items to the dictionary
                            }
                            else if (typeof(Enum).IsAssignableFrom(prop.PropertyType))
                            {
                                string enumValue = null;
                                if (embeddedValue != null)
                                {
                                    enumValue = embeddedValue;
                                }
                                else
                                {
                                    var collectedValues = GetValueFromSubsequentArgs(argList, argList.IndexOf(arg), 1);
                                    if (collectedValues.Count > 0)
                                    {
                                        enumValue = collectedValues[0];
                                    }
                                }

                                if (enumValue != null)
                                {
                                    var typedEnumValue = Enum.Parse(prop.PropertyType, enumValue, false);
                                    if (typedEnumValue != null)
                                    {
                                        prop.SetValue(command, typedEnumValue, null);
                                    }
                                }
                            }
                            else if (prop.PropertyType == typeof(bool))
                            {
                                // set to TRUE since we found an arg with this name
                                prop.SetValue(command, true, null);
                            }
                            else
                            {
                                if (embeddedValue != null)
                                {
                                    prop.SetValue(command, Convert.ChangeType(embeddedValue, prop.PropertyType), null);
                                }
                                else
                                {
                                    var collectedValues = GetValueFromSubsequentArgs(argList, argList.IndexOf(arg), 1);
                                    if (collectedValues.Count > 0)
                                    {
                                        prop.SetValue(command, Convert.ChangeType(collectedValues[0], prop.PropertyType), null);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region - Assign positional args -

            // Assign position numbers such that named args
            // and their subsequent values only consume 1 position.
            var positionCounter = 0;
            for (int i = 0; i < argList.Count; i++)
            {
                if (argList[i].IsName)
                {
                    argList[i].Position = positionCounter;
                    positionCounter++;
                }
                else if (!argList[i].Used)
                {
                    argList[i].Position = positionCounter;
                    positionCounter++;
                }
            }

            // remove any used args
            argList.RemoveAll(x => x.Used);

            // if there are any unused arguments left
            if (argList.Count > 0)
            {
                // for each of the command's parameters
                foreach (var prop in parameterProps.Keys)
                {
                    // skip any parameters we have already processed
                    if (processedParameters.Contains(prop))
                        continue;

                    var paramInfo = parameterProps[prop];

                    // if the parameter accepts a positional value
                    if (paramInfo.Position >= 0)
                    {
                        var arg = argList.SingleOrDefault(x => x.Position == paramInfo.Position);
                        // and if there is an arg at that position)
                        if (arg != null)
                        {
                            // note that even though we mark it as used (to exclude from the catch all), positional args can be reused (ex: 2 parameters can collect an arg in position 0)
                            arg.Used = true;
                            var value = arg.Arg;

                            // note that lists and dictionaries are not supported for positional args
                            if (typeof(Enum).IsAssignableFrom(prop.PropertyType))
                            {
                                var typedEnumValue = Enum.Parse(prop.PropertyType, value, false);
                                if (typedEnumValue != null)
                                {
                                    prop.SetValue(command, typedEnumValue, null);
                                }
                            }
                            else if (prop.PropertyType == typeof(bool))
                            {
                                // set to TRUE since we found an arg with this name
                                prop.SetValue(command, true, null);
                            }
                            else
                            {
                                command.SetParameterValue(prop, value);
                            }
                        }
                    }
                }
            }

            #endregion

            #region - Add remaining args to catch-all parameter(s) -

            // remove any used args
            argList.RemoveAll(x => x.Used);

            // look for catch-all parameters
            foreach (var prop in parameterProps.Keys)
            {
                var paramInfo = parameterProps[prop];
                if (paramInfo.IsCatchAll)
                {
                    // the catch-all parameter must be a list of strings
                    if (typeof(IList<string>).IsAssignableFrom(prop.PropertyType))
                    {
                        var list = (IList<string>)prop.GetValue(command, null);

                        // add all of the remaining args to the catch-all list
                        foreach (var arg in argList)
                        {
                            list.Add(arg.Arg);
                        }
                    }
                }
            }

            #endregion
        }

        private static IList<string> GetValueFromSubsequentArgs(IList<ParsableArg> argList, int startingIndex, int limit)
        {
            var values = new List<string>();

            for (int i = startingIndex; i < argList.Count; i++)
            {
                if (argList[i].Used)
                    continue;

                var arg = argList[i].Arg;

                // stop collecting values when we hit another named arg
                if (IsNamedArg(arg))
                {
                    break;
                }

                // add value to the list
                values.Add(arg);
                argList[i].Used = true;

                // stop collecting values when we reach the limit
                if (values.Count == limit)
                    break;
            }

            return values;
        }

        private static string GetValueFromNamedArg(string arg)
        {
            string value = null;
            var index = arg.IndexOfAny(new[] { ':', '=' });
            if (index > -1)
            {
                value = arg.Substring(index + 1);
            }

            return value;
        }

        private static bool IsNamedArg(string arg)
        {
            bool named = arg.StartsWith("--") || arg.StartsWith("/");
            return named;
        }

        private static bool IsMatchingNamedArg(string arg, string nameToMatch, char flag)
        {
            if (IsNamedArg(arg))
            {
                // Note: the name matching is case-insensitive but the FLAG is case-sensitive

                var name = arg.TrimStart(new[] { '-', '/' });
                if (string.Equals(name, nameToMatch, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, flag.ToString(), StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }
}