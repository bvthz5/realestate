namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IEmailService
    {
        void ChangeStatusUser(string? email, string? status, string? firstname);
        void ChangeStatusEnquiry(string? email, string? status, string? name, string? enquiryType);
        public interface IEmailService
        {
            Task<bool> SendEmailAsync(string? to, string? subject, string? body);
            void VerifyUser(string? to, string? token);
            void ForgotPassword(string? to, string? token);
        }
    }
}
