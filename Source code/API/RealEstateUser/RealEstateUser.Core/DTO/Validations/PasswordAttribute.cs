using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RealEstateUser.Core.DTO.Validations
{
    public class PasswordAttribute : ValidationAttribute
    {
        private readonly string _pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$&+,:;=?@#|'<>.-^*()%!])[A-Za-z\\d$&+,:;=?@#|'<>.-^*()%!]{8,16}$";

        public override bool IsValid(object? value)
        {
            if (value == null || (string)value == string.Empty)
            {
                ErrorMessage = "Password is Required";
                return false;
            }
            if (((string)value).Length > 16 || ((string)value).Length < 8)
            {
                ErrorMessage = "Password Length should be 8 - 16";
                return false;
            }

            ErrorMessage = "Must contain at least one uppercase letter, one lowercase letter, one number and one special character";
            return Regex.IsMatch((string)value, _pattern);
        }
    }
}
