using System;

namespace DotConsole.Helpers
{
    internal static class TypeHelpers
    {
        public static object GetDefaultValue(this Type type)
        {
            if (!type.IsValueType)
            {
                return null;
            }

            return Activator.CreateInstance(type);
        }
    }
}