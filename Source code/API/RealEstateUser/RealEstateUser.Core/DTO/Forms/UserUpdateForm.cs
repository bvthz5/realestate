
using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Validations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class UserUpdateForm
    {


        [StringLength(50)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Invalid first name format. Please enter only letters.")]

        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Invalid last name format. Please enter only letters.")]

        public string? LastName { get; set; }

        [StringLength(100)]
        [Address]
        public string? Address { get; set; }

        [Validations.Phone(Nullable = true)]
        public string? PhoneNumber { get; set; }
    }
}
