using System;
using RealEstate.Domain.Data.Entities;
using Xunit;

namespace RealEstateUser.Test.API.Entities
{
    public class EnquiryTests
    {
        [Fact]
        public void Enquiry_DefaultValues_AreCorrect()
        {
            // Arrange and Act
            var enquiry = new Enquiry();

            // Assert
            Assert.True(enquiry.IsActive);
            Assert.False(enquiry.IsDeleted);
            Assert.Equal(DateTime.Now.Date, enquiry.CreatedOn.Date);
            Assert.Equal(default(DateTime), enquiry.UpdatedOn);
            Assert.Equal(default(int), enquiry.CreatedBy);
            Assert.Null(enquiry.UpdatedBy);
        }

        [Fact]
        public void Enquiry_PropertyValues_AreSetCorrectly()
        {
            // Arrange
            var name = "Test Name";
            var email = "test@example.com";
            var message = "Test Message";
            var enquiryType = (byte)1;
            var availableDates = DateTime.Now.Date.AddDays(7);
            var availableTime = "10:00 AM";
            var phoneNumber = "1234567890";
            var status = (byte)2;

            // Act
            var enquiry = new RealEstate.Domain.Data.Entities.Enquiry
            {
                Name = name,
                Email = email,
                Message = message,
                EnquiryType = enquiryType,
                AvailableDates = availableDates,
                AvailableTime = availableTime,
                PhoneNumber = phoneNumber,
                Status = status
            };

            // Assert
            Assert.Equal(name, enquiry.Name);
            Assert.Equal(email, enquiry.Email);
            Assert.Equal(message, enquiry.Message);
            Assert.Equal(enquiryType, enquiry.EnquiryType);
            Assert.Equal(availableDates, enquiry.AvailableDates);
            Assert.Equal(availableTime, enquiry.AvailableTime);
            Assert.Equal(phoneNumber, enquiry.PhoneNumber);
            Assert.Equal(status, enquiry.Status);
        }

    }
}
