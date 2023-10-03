using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RealEstateAdmin.Core.DTO.Validations
{
    public class PasswordAttribute : ValidationAttribute
    {
        private readonly string _pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&#])[A-Za-z\\d@$!%*?#&]{8,16}$";

        public override bool IsValid(object? value)
        {
            if (value == null || (string)value == string.Empty)
            {
                ErrorMessage = "Password is Required";
                return false;
            }
            if (((string)value).Length > 16 || ((string)value).Length < 8)
            {
                ErrorMessage = "Invalid Credentials";
                return false;
            }

            ErrorMessage = "Invalid Credentials";
            return Regex.IsMatch((string)value, _pattern);
        }
    }
}

