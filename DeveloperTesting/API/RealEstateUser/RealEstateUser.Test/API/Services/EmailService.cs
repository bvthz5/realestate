/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RealEstateUser.Core.Services;
using RealEstateUser.Core.Settings;
using Xunit;

namespace RealEstateUser.Test.API.Services
{
    public class EmailServiceTests
    {
        private readonly IOptions<MailSettings> _mailSettings;
        private readonly Mock<ILogger<EmailService>> _logger;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _mailSettings = Options.Create(new MailSettings
            {
                DisplayName = "Test Mail",
                Mail = "binilvincent80@gmail.com",
                Pswd = "p@sswo3d",
                Host = "smtp.test.com",
                Port = 587
            });
            _logger = new Mock<ILogger<EmailService>>();
            _emailService = new EmailService(_mailSettings, _logger.Object);
        }
        [Fact]
        public async Task SendEmailAsync_InvalidInputs_ReturnsFalse()
        {
            // arrange
            var to = "invalid";
            var subject = "";
            var body = "";

            // act
            var result = await _emailService.SendEmailAsync(to, subject, body);

            // assert
            Assert.False(result);
        }


    }

}
*/