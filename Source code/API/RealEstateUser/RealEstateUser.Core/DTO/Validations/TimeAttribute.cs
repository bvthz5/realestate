using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Forms;

namespace RealEstateUser.Core.DTO.Validations
{
    public class TimeAttribute : ValidationAttribute
    {
        private readonly int _maxDays = 14;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string time)
            {
                return new ValidationResult("Invalid Time format.");
            }

            if (!TimeOnly.TryParse(time, out TimeOnly selectedTime))
            {
                return new ValidationResult("Invalid Time format. Please use the format HH:mm (e.g. 08:30).");
            }

            if (validationContext.ObjectInstance is not EnquiryTourForm form)
            {
                return new ValidationResult("Invalid input type.");
            }

            if (form.AvailableDates == null)
            {
                return new ValidationResult("Available date is required.");
            }

            var selectedDateTime = new DateTime(form.AvailableDates.Value.Year, form.AvailableDates.Value.Month, form.AvailableDates.Value.Day,
                                                 selectedTime.Hour, selectedTime.Minute, 0);

            var minDateTime = DateTime.Now;
            var maxDateTime = minDateTime.AddDays(_maxDays).AddHours(12); // Add 12 hours to include full day of _maxDays

            if (selectedDateTime < minDateTime)
            {
                return new ValidationResult("Cannot select a date and time in the past.");
            }

            if (selectedDateTime > maxDateTime)
            {
                return new ValidationResult($"Available date and time should be within {_maxDays} days of the current date.");
            }

            return ValidationResult.Success;
        }

    }
}
