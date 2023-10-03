using System;
using RealEstate.Domain.Data.Entities;
using Xunit;

namespace RealEstateUser.Test.API.Entities
{
    public class AdminTest
    {
        public class AdminTests
        {
            [Fact]
            public void Admin_SetProperties_ValidValues()
            {
                // Arrange
                var admin = new Admin();
                var name = "John Doe";
                var email = "johndoe@example.com";
                var password = "password123";
                var profilePic = "https://example.com/profile.jpg";
                var verificationCode = "ABC123";
                var createdDate = DateTime.UtcNow;
                var updatedDate = DateTime.UtcNow;

                // Act
                admin.Name = name;
                admin.Email = email;
                admin.Password = password;
                admin.ProfilePic = profilePic;
                admin.VerificationCode = verificationCode;
                admin.CreatedDate = createdDate;
                admin.UpdatedDate = updatedDate;

                // Assert
                Assert.Equal(name, admin.Name);
                Assert.Equal(email, admin.Email);
                Assert.Equal(password, admin.Password);
                Assert.Equal(profilePic, admin.ProfilePic);
                Assert.Equal(verificationCode, admin.VerificationCode);
                Assert.Equal(createdDate, admin.CreatedDate);
                Assert.Equal(updatedDate, admin.UpdatedDate);
            }
        }

    }
}
