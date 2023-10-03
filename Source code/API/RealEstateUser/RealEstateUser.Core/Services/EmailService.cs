using System.Text;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RealEstateUser.Core.ServiceContracts;
using RealEstateUser.Core.Settings;

namespace RealEstateUser.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        private readonly ILogger<EmailService> _logger;

        private const string LogErrorMessageTemplate = "Error: {Message} To: {To} Subject: {Subject} Exception: {Exception}";

        private const string ErrorMessageTemplate = "Verify Email Template Error: {ErrorMessage}";

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends an email to the specified recipient with the specified subject and body.
        /// </summary>
        /// <param name="to">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        /// <returns>Returns true if the email was sent successfully; otherwise, false.</returns>

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
/*            await Task.Delay(100);
            return true;*/
            try
            {
                // create a new MimeMessage
                var email = new MimeMessage();

                // set the from address to the mail settings display name and email address
                email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

                // add the recipient's email address to the To field
                email.To.Add(MailboxAddress.Parse(to));

                // set the subject of the email
                email.Subject = subject;

                // set the body of the email as a TextPart with HTML format
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                // create a new SmtpClient and connect to the email server
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                // authenticate with the email server using the mail settings email and password
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Pswd);

                // send the email
                await smtp.SendAsync(email);

                // disconnect from the email server
                smtp.Disconnect(true);

                // return true to indicate that the email was sent successfully
                return true;

            }
            catch (Exception e)
            {
                // log the error message and return false to indicate that the email was not sent
                _logger.LogError(LogErrorMessageTemplate, e.Message, to, subject, e);
                return false;
            }
        }

        /// <summary>
        /// Sends an email to a user for email verification.
        /// </summary>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="token">The email verification token.</param>

        public async void VerifyUser(string to, string token)
        {
            try
            {
                // Define the email subject.
                var subject = "Email Verification";

                // Read the HTML email template from file.
                var htmlContent = new StringBuilder(await System.IO.File.ReadAllTextAsync(Directory.GetCurrentDirectory() + @"/wwwroot/Template/EmailTemplate.txt"));

                // Replace the placeholders in the email template with the actual values.
                htmlContent.Replace("@#title#@", "Email Verification");
                htmlContent.Replace("@#message#@", @"Tap the button below to confirm your email address");

                htmlContent.Replace("@#token_url#@", $"{_mailSettings.ClientBaseUrl}verify?token={token}");
                htmlContent.Replace("@#base_url#@", _mailSettings.ClientBaseUrl);

                htmlContent.Replace("@#button#@", "Click here to Verify");

                htmlContent.Replace("@#name#@", _mailSettings.DisplayName);
                htmlContent.Replace("@#logo_url#@", @"Logo");

                // Send the email.
                await SendEmailAsync(to, subject, htmlContent.ToString());
            }
            catch (Exception e)
            {
                // Log any errors
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
        }

        /// <summary>
        /// Sends a forgot password email to the specified email address with a password reset token.
        /// </summary>
        /// <param name="to">The email address to send the email to.</param>
        /// <param name="token">The password reset token.</param>

        public async void ForgotPassword(string to, string token)
        {
            try
            {
                // Set email subject
                var subject = "Reset Password";

                // Load email template
                var htmlContent = new StringBuilder(await System.IO.File.ReadAllTextAsync(Directory.GetCurrentDirectory() + @"/wwwroot/Template/EmailTemplate.txt"));

                // Replace email template variables with appropriate values
                htmlContent.Replace("@#title#@", "Reset Password");
                htmlContent.Replace("@#message#@", "Tap the button below to reset your password.");

                // Add password reset token to email template
                htmlContent.Replace("@#token_url#@", $"{_mailSettings.ClientBaseUrl}forgot-password?token={token}");
                htmlContent.Replace("@#base_url#@", _mailSettings.ClientBaseUrl);

                // Set button text in email template
                htmlContent.Replace("@#button#@", "Reset Password");

                // Set display name in email template
                htmlContent.Replace("@#name#@", _mailSettings.DisplayName);

                // Send the email
                await SendEmailAsync(to, subject, htmlContent.ToString());
            }
            catch (Exception e)
            {
                // Log any errors
                _logger.LogError("Forgot Password Email Template Error ", e);
            }
        }

        public async void TourRequest(string email, string? enquiryType, string? enquiryStatus, string? name)
        {
            try
            {
                string sub = $"You have a {enquiryStatus}";
                string body = $@"
                {enquiryStatus}
                ";
                await SendEmailAsync(email, sub, body);
            }
            catch (Exception e)
            {
                _logger.LogError("Request Mail Error", e);
            }
        }
    }
}

