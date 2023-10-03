namespace RealEstateUser.Core.ServiceContracts
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        void TourRequest(string email, string? enquiryType, string? enquiryStatus, string? name);
        void VerifyUser(string to, string token);

        void ForgotPassword(string to, string token);
    }
}
