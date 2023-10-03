using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<ServiceResult> Login(LoginForm form);
        Task<ServiceResult> Refresh(string token);
        Task<ServiceResult> AddUser(UserRegistrationForm form);
        Task<ServiceResult> ResendVerificationMail(string email);
        Task<ServiceResult> VerifyUser(string token);
        Task<ServiceResult> ForgotPasswordRequest(string email);
        Task<ServiceResult> ResetPassword(ForgotPasswordForm form);
        Task<ServiceResult> UploadImage(int userId, ImageForm image);
        Task<FileStream?> GetProfile(string fileName);
        Task<ServiceResult> ChangePassword(ChangePasswordForm form, int userId);
        Task<ServiceResult> GetUserAsync(int userId);
        Task<ServiceResult> EditAsync(int userId, UserUpdateForm form);
    }
}
