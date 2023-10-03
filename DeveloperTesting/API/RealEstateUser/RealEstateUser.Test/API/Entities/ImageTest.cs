using RealEstate.Domain.Data.Entities;
using Xunit;

namespace RealEstateUser.Test.API.Entities
{
    public class ImageTests
    {
        [Fact]
        public void Image_DefaultValues_AreCorrect()
        {
            // Arrange and Act
            var image = new Image();

            // Assert
            Assert.Equal(default(int), image.ImageId);
            Assert.Equal(default(int), image.PropertyId);
            Assert.Null(image.Property);
            Assert.Null(image.PropertyImages);
        }

        [Fact]
        public void Image_PropertyValues_AreSetCorrectly()
        {
            // Arrange
            var propertyId = 123;
            var propertyImages = "image1.jpg";

            // Act
            var image = new Image
            {
                PropertyId = propertyId,
                PropertyImages = propertyImages
            };

            // Assert
            Assert.Equal(propertyId, image.PropertyId);
            Assert.Equal(propertyImages, image.PropertyImages);
        }
    }
}
