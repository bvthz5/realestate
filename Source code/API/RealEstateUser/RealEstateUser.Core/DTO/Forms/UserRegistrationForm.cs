using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Validations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class UserRegistrationForm
    {
        [Email]
        public string Email { get; set; } = string.Empty;

        [Password]
        public string Password { get; set; } = string.Empty;
    }
}
