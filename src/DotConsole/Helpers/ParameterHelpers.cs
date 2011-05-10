using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DotConsole.Helpers
{
    internal static class ParameterHelpers
    {
        public static string GetDescription(this PropertyInfo property)
        {
            var desc = property.GetCustomAttributes(typeof(DescriptionAttribute), true)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault();

            return desc;
        }

        public static bool IsRequired(this PropertyInfo property)
        {
            bool required = property.GetCustomAttributes(typeof(RequiredAttribute), false)
                                .Count() != 0;
            return required;
        }
    }
}