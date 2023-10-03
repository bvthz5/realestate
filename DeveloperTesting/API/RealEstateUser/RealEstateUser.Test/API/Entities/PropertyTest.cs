using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RealEstate.Domain.Data.Entities;
using Xunit;

namespace RealEstateUser.Test.API.Entities
{
    public class PropertyTests
    {
        [Fact]
        public void Property_DefaultValues_AreCorrect()
        {
            // Arrange and Act
            var property = new Property();

            // Assert
            Assert.Equal(default(int), property.PropertyId);
            Assert.Null(property.Description);
            Assert.Null(property.Address);
            Assert.Equal(string.Empty, property.City);
            Assert.Equal(default(int), property.CategoryId);
            Assert.Null(property.Category);
            Assert.Equal(default(byte), property.Status);
            Assert.Equal(default(float), property.Price);
            Assert.Equal(default(double), property.Longitude);
            Assert.Equal(default(double), property.Latitude);
            Assert.Equal(string.Empty, property.ZipCode);
            Assert.Equal(default(int), property.TotalBedrooms);
            Assert.Equal(default(int), property.TotalBathrooms);
            Assert.Equal(default(float), property.MonthlyRent);
            Assert.Equal(default(float), property.SquareFootage);
            Assert.Equal(default(float), property.SecurityDeposit);
        }

        [Fact]
        public void Property_PropertyValues_AreSetCorrectly()
        {
            // Arrange
            var description = "A beautiful apartment";
            var address = "123 Main St";
            var city = "New York";
            var categoryId = 1;
            var category = new Category { CategoryId = categoryId };
            var status = (byte)2;
            var price = 2000.50f;
            var longitude = 40.7128;
            var latitude = -74.0060;
            var zipCode = "10001";
            var totalBedrooms = 2;
            var totalBathrooms = 2;
            var monthlyRent = 1500.50f;
            var squareFootage = 1000.00f;
            var securityDeposit = 1000.00f;

            // Act
            var property = new Property
            {
                Description = description,
                Address = address,
                City = city,
                CategoryId = categoryId,
                Category = category,
                Status = status,
                Price = price,
                Longitude = longitude,
                Latitude = latitude,
                ZipCode = zipCode,
                TotalBedrooms = totalBedrooms,
                TotalBathrooms = totalBathrooms,
                MonthlyRent = monthlyRent,
                SquareFootage = squareFootage,
                SecurityDeposit = securityDeposit
            };

            // Assert
            Assert.Equal(description, property.Description);
            Assert.Equal(address, property.Address);
            Assert.Equal(city, property.City);
            Assert.Equal(categoryId, property.CategoryId);
            Assert.Equal(category, property.Category);
            Assert.Equal(status, property.Status);
            Assert.Equal(price, property.Price);
            Assert.Equal(longitude, property.Longitude);
            Assert.Equal(latitude, property.Latitude);
            Assert.Equal(zipCode, property.ZipCode);
            Assert.Equal(totalBedrooms, property.TotalBedrooms);
            Assert.Equal(totalBathrooms, property.TotalBathrooms);
            Assert.Equal(monthlyRent, property.MonthlyRent);
            Assert.Equal(squareFootage, property.SquareFootage);
            Assert.Equal(securityDeposit, property.SecurityDeposit);
        }

        [Fact]
        public void Property_DescriptionIsRequired()
        {
            // Arrange
            var property = new Property();

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(property, null, null);
            var isValid = Validator.TryValidateObject(property, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(Property.Description)));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(Property.Address)));
        }


        [Fact]
        public void Property_AddressIsRequired()
        {
            // Arrange
            var property = new Property();

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(property, null, null);
            var isValid = Validator.TryValidateObject(property, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(Property.Address)));
        }
    }
}
