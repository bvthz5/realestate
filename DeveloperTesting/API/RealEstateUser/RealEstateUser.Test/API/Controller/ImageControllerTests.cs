using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RealEstateUser.Api.Controllers;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;
using Xunit;

namespace RealEstateUser.Test.API.Controller
{
    public class ImageControllerTests
    {
        private readonly IImageService _imageService = Substitute.For<IImageService>();
        ImageController? controller;

        //Gets the photos for the specified product.

        [Fact]
        public async Task GetPhotos_ValidPropertyId_ReturnsSuccess()
        {
            // Arrange
            int propertyId = 1;
            ServiceResult result = new() { ServiceStatus = ServiceStatus.Success };
            _imageService.GetPhotosAsync(propertyId).Returns(result);
            controller = new ImageController(_imageService);

            // Act
            IActionResult response = await controller.GetPhotos(propertyId);
            ObjectResult? objectResult = response as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.Equal((int)ServiceStatus.Success, objectResult?.StatusCode);
            Assert.Equal(result, objectResult?.Value);
        }

        [Fact]
        public async Task GetPhotos_InvalidPropertyId_ReturnsBadRequest()
        {
            // Arrange
            int propertyId = 0;
            controller = new ImageController(_imageService);

            // Act
            IActionResult response = await controller.GetPhotos(propertyId);
            BadRequestResult? badRequestResult = response as BadRequestResult;

            // Assert
            Assert.NotNull(badRequestResult);
        }

        //Gets the photo(s) for the specified file path.

/*        [Fact]
        public void GetPhotosByPath_ValidFilePath_ReturnsFileStreamResult()
        {
            // Arrange
            string filePath = "image.jpeg";
            FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath), FileMode.Open);
            _imageService.GetPhotosByName(filePath).Returns(fileStream);
            controller = new ImageController(_imageService);

            // Act
            IActionResult? response = controller.GetPhotosByPath(filePath);
            FileStreamResult fileStreamResult = response as FileStreamResult;

            // Assert
            Assert.NotNull(fileStreamResult);
            Assert.Equal("image/jpeg", fileStreamResult?.ContentType);
            FileStream returnedFileStream = fileStreamResult?.FileStream as FileStream;
            Assert.NotNull(returnedFileStream);
            Assert.Equal(fileStream.Length, returnedFileStream.Length);
        }
*/


        [Fact]
        public void GetPhotosByPath_InvalidFilePath_ReturnsNotFoundResult()
        {
            // Arrange
            string filePath = "invalid.jpg";
            _imageService.GetPhotosByName(filePath).Returns((FileStream)null);
            controller = new ImageController(_imageService);

            // Act
            IActionResult response = controller.GetPhotosByPath(filePath);
            NotFoundResult? notFoundResult = response as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
        }

        // Gets the video(s) for the specified file path.

  /*      [Fact]
        public void GetVideoByPath_ValidFilePath_ReturnsFileStreamResult()
        {
            // Arrange
            string filePath = "video.mp4";
            FileStream fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath), FileMode.Open);
            _imageService.GetVideosByName(filePath).Returns(fileStream);
            controller = new ImageController(_imageService);

            // Act
            IActionResult? response = controller.GetVideoByName(filePath);
            FileStreamResult fileStreamResult = response as FileStreamResult;

            // Assert
            Assert.NotNull(fileStreamResult);
            Assert.Equal("video/mp4", fileStreamResult?.ContentType);
            FileStream returnedFileStream = fileStreamResult.FileStream as FileStream;
            Assert.NotNull(returnedFileStream);
            Assert.Equal(fileStream.Length, returnedFileStream.Length);
        }*/



        [Fact]
        public void GetVideoByName_InvalidFilePath_ReturnsNotFoundResult()
        {
            // Arrange
            string filePath = "invalid.mp4";
            _imageService.GetVideosByName(filePath).Returns((FileStream)null);
            controller = new ImageController(_imageService);

            // Act
            IActionResult? response = controller.GetVideoByName(filePath);
            NotFoundResult? notFoundResult = response as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
        }
    }

}
