using RealEstateAdmin.Core.DTO.Validations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class LoginForm
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
