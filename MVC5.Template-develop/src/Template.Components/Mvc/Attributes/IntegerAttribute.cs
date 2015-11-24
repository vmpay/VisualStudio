using Template.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Template.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class IntegerAttribute : ValidationAttribute
    {
        public IntegerAttribute() : base(() => Validations.FieldMustBeInteger)
        {
        }

        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return true;

            return Regex.IsMatch(value.ToString(), "^[+-]?[0-9]+$");
        }
    }
}
