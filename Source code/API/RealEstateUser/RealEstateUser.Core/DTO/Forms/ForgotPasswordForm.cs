using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Validations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class ForgotPasswordForm
    {
        [Required(ErrorMessage = "Token is required")]
        [StringLength(255)]
        public string Token { get; set; } = null!;

        [Password]
        public string Password { get; set; } = string.Empty;

        [Password]
        [Compare("Password", ErrorMessage = "Confirm Password does not match ")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
