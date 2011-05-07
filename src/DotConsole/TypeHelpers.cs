using System;

namespace DotConsole
{
    public static class TypeHelpers
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