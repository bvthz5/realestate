using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Settings;

namespace RealEstateAdmin.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;
        private const string LogErrorMessageTemplate = "Error: {Message} To: {To} Subject: {Subject} Exception: {Exception}";
        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            /*await Task.Delay(100);
            return true;
*/
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Pswd);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(LogErrorMessageTemplate, e.Message, to, subject, e);
                return false;
            }
        }

        /// <summary>
        /// Change User Status
        /// </summary>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="firstname"></param>
        public async void ChangeStatusUser(string? email, string? status, string? firstname)
        {
            try
            {
                if(email == null)
                {
                    return;
                }
                string subject = $" Real Estate. Request {status}";
                string body = @$"
                Hi {firstname},
                <br>
                Your Account is {status}
                <br>
                ";
                await SendEmailAsync(email, subject, body);
            }
            catch (Exception e)
            {
                _logger.LogError("Request Mail Error ", e);
            }
        }

        /// <summary>
        /// Change Enquiry Status
        /// </summary>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="name"></param>
        /// <param name="enquiryType"></param>
        public async void ChangeStatusEnquiry(string? email, string? status, string? name, string? enquiryType)
        {
            try
            {
                if(email == null)
                {
                    return;
                }
                string subject = $" Enquiry {status}";
                string body = @$"
                Hi {name},
                <br>
                Your Enquiry for {enquiryType} is {status}
                <br>
                ";
                await SendEmailAsync(email, subject, body);
            }
            catch (Exception e)
            {
                _logger.LogError("Request Mail Error ", e);
            }
        }
    }
}

