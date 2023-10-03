using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Validations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class ChangePasswordForm
    {

        public string CurrentPassword { get; set; } = string.Empty;

        [Password]
        [NotEqualTo("CurrentPassword")]
        public string NewPassword { get; set; } = string.Empty;

        [Password]
        [Compare("NewPassword", ErrorMessage = "Confirm Password does not match ")]
        public string ConfirmNewPassword { get; set; } = string.Empty;

    }
}
