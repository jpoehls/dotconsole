﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DotConsole
{
    public class DataAnnotationValidator : ICommandValidator
    {
        private readonly List<string> _errors;

        public DataAnnotationValidator()
        {
            _errors = new List<string>();
        }

        public IEnumerable<string> ErrorMessages { get { return _errors; } }

        public virtual bool ValidateParameters(ICommand command)
        {
            _errors.Clear();

            var parameters = command.GetParameters();

            foreach (PropertyInfo prop in parameters.Keys)
            {
                IEnumerable<ValidationAttribute> validationAttrs =
                    prop.GetCustomAttributes(typeof(ValidationAttribute), true)
                        .OfType<ValidationAttribute>();

                foreach (ValidationAttribute attr in validationAttrs)
                {
                    if (!attr.IsValid(prop.GetValue(this, null)))
                    {
                        _errors.Add(attr.ErrorMessage);
                    }
                }
            }

            return _errors.Count == 0;
        }
    }
}