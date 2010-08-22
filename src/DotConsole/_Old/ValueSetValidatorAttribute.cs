using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotConsole
{
    /// <summary>
    /// Validates that the given value matches one of the valid values in the set.
    /// </summary>
    public class ValueSetValidatorAttribute : ValidationAttribute
    {
        public ValueSetValidatorAttribute(params string[] validValues)
        {
            ValidValues = validValues;
        }

        public string[] ValidValues { get; private set; }

        public override bool IsValid(object value)
        {
            string sValue = (value != null) ? value.ToString() : string.Empty;
            return ValidValues.Contains(sValue, StringComparer.OrdinalIgnoreCase);
        }
    }
}