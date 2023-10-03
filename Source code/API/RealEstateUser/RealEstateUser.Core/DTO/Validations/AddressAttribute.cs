using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RealEstateUser.Core.DTO.Validations
{
    public class AddressAttribute : ValidationAttribute
    {
        private readonly string _pattern = "^[A-Za-z0-9()_\\-,.'&+:/ \\n\\s]+$";

        public override bool IsValid(object? value)
        {
            string inputValue = value?.ToString() ?? "";
            ErrorMessage = "Validation Failed";
            return Regex.IsMatch(inputValue, _pattern, RegexOptions.IgnoreCase);
        }
    }

}
