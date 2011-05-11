using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DotConsole.Helpers;

namespace DotConsole
{
    /// <summary>
    /// Validates all <see cref="ICommand"/> properties marked with <see cref="ParameterAttribute"/>
    /// and collects any validation error messages.
    /// </summary>
    public class DataAnnotationValidator : ICommandValidator
    {
        protected const string DefaultErrorMessage = "validation failed";

        private readonly List<string> _errors;

        public DataAnnotationValidator()
        {
            _errors = new List<string>();
        }

        public IEnumerable<string> ErrorMessages { get { return _errors; } }

        public virtual bool ValidateParameters(ICommand command)
        {
            _errors.Clear();

            bool valid = true;

            var parameters = command.GetParameters();

            foreach (PropertyInfo prop in parameters.Keys)
            {
                IEnumerable<ValidationAttribute> validationAttrs =
                    prop.GetCustomAttributes(typeof(ValidationAttribute), true)
                        .OfType<ValidationAttribute>();

                foreach (ValidationAttribute attr in validationAttrs)
                {
                    if (!attr.IsValid(prop.GetValue(command, null)))
                    {
                        valid = false;
                        if (!string.IsNullOrWhiteSpace(attr.ErrorMessage))
                        {
                            _errors.Add(attr.ErrorMessage);
                        }
                    }
                }
            }

            // add a default error message if validation failed
            // and no specific error messages were provided
            if (!valid && _errors.Count == 0)
            {
                _errors.Add(DefaultErrorMessage);
            }

            return valid;
        }
    }
}