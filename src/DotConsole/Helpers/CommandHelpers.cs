using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DotConsole.Helpers
{
    internal static class CommandHelpers
    {
        public static void SetParameterValue(this ICommand command, PropertyInfo parameterProperty, string value)
        {
            if (parameterProperty == null)
                return;

            if (parameterProperty.PropertyType == typeof(string))
            {
                parameterProperty.SetValue(command, value, null);
            }
            else
            {
                object convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(value, parameterProperty.PropertyType);
                }
                catch (Exception)
                {
                    Trace.TraceInformation("Failed to convert string value to {0}. [ Value: \"{1}\" ]",
                                           parameterProperty.PropertyType.FullName, value);
                    convertedValue = parameterProperty.PropertyType.GetDefaultValue();
                }
                parameterProperty.SetValue(command, convertedValue , null);
            }
        }

        public static Dictionary<PropertyInfo, ParameterAttribute> GetParameters(this ICommand command)
        {
            var type = command.GetType();
            Dictionary<PropertyInfo, ParameterAttribute> p = type.GetProperties(BindingFlags.Public |
                                                                                BindingFlags.Instance)
                .Select(x => new
                                 {
                                     Property = x,
                                     Attribute =
                                 (ParameterAttribute)x.GetCustomAttributes(typeof(ParameterAttribute), false)
                                                          .FirstOrDefault()
                                 })
                .Where(x => x.Attribute != null)
                .OrderBy(x => x.Attribute.Position)
                .ToDictionary(x => x.Property, x => x.Attribute);
            return p;
        }

        public static string GetDescription(this ICommand command)
        {
            var type = command.GetType();
            var desc = type.GetCustomAttributes(typeof(DescriptionAttribute), true)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault();

            return desc;
        }
    }
}