using System.ComponentModel.DataAnnotations;

namespace RealEstateUser.Core.DTO.Validations
{
    public class NotEqualToAttribute : ValidationAttribute
    {
        private string OtherProperty { get; set; }

        public NotEqualToAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // get other property value
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

            // use null-conditional operator to check if otherPropertyInfo is null
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

            // verify values
            if (value?.ToString()?.Equals(otherValue?.ToString()) ?? false)
            {
                return new ValidationResult(string.Format("{0} should not be equal to {1}.", validationContext.MemberName, OtherProperty));
            }

            return ValidationResult.Success;
        }
    }
}
