using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RealEstate.Domain.Data.Entities;
using Xunit;

namespace RealEstateUser.Test.API.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void Category_DefaultValues_AreCorrect()
        {
            // Arrange and Act
            var category = new Category();

            // Assert
            Assert.True(category.IsActive);
            Assert.False(category.IsDeleted);
            Assert.Equal(DateTime.Now.Date, category.CreatedOn.Date);
            Assert.Equal(default(DateTime), category.UpdatedOn);
            Assert.Equal(default(int), category.CreatedBy);
            Assert.Null(category.UpdatedBy);
        }

        [Fact]
        public void Category_PropertyValues_AreSetCorrectly()
        {
            // Arrange
            var categoryName = "Test Category";
            var type = (byte)1;
            var status = (byte)2;

            // Act
            var category = new Category
            {
                CategoryName = categoryName,
                Type = type,
                Status = status
            };

            // Assert
            Assert.Equal(categoryName, category.CategoryName);
            Assert.Equal(type, category.Type);
            Assert.Equal(status, category.Status);
        }

        [Fact]
        public void Category_CategoryNameIsRequired()
        {
            // Arrange
            var category = new Category();

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(category, null, null);
            var isValid = Validator.TryValidateObject(category, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(Category.CategoryName)));
        }
    }

}
