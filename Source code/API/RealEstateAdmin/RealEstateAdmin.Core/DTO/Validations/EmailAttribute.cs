using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RealEstateAdmin.Core.DTO.Validations
{
    public class EmailAttribute : ValidationAttribute
    {
        private readonly string _pattern = @"^[A-Z0-9._%+-]+@[A-z0-9.-]+\.[A-Z]{2,254}$";

        public override bool IsValid(object? value)
        {
            if (value == null || (string)value == string.Empty)
            {
                ErrorMessage = "Email Is Required";
                return false;
            }

            if (((string)value).Length > 255)
            {
                ErrorMessage = "Email Length should be less than 255";
                return false;
            }
            ErrorMessage = "Not a Valid Email Address";
            return Regex.IsMatch((string)value, _pattern, RegexOptions.IgnoreCase);
        }
    }
}