using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RealEstateUser.Core.DTO.Validations
{
    public class DateAttribute : ValidationAttribute
    {
        private readonly int _maxDays = 14;

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is DateTime dateValue)
            {
                if (dateValue.Date < DateTime.Now.Date)
                {
                    ErrorMessage = "Available Date cannot be a past date";
                    return false;
                }

                if (dateValue.Date > DateTime.Now.Date.AddDays(_maxDays))
                {
                    ErrorMessage = $"Available Date should be within {_maxDays} days of the current date";
                    return false;
                }

                return true;
            }

            if (DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }

            ErrorMessage = "Invalid Date Format";
            return false;
        }
    }
}



