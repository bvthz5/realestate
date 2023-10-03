using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Validations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class LoginForm
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

