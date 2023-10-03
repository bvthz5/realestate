using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RealEstateAdmin.Api.Controllers;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Test.API.Controller
{
    public class ImageControllerTests
    {
        private readonly IImageService _imageService = Substitute.For<IImageService>();
        ImageController? controller;
        [Fact]
        public async Task When_Add_Success()
        {
            // Arrange
            PropertyImageForm imageForm = new();
            PropertyVideoForm videoForm = new();
            int propertyId = 1;
            ServiceResult expectedResult = new();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK; // Set the expected status code
            _imageService.AddPhotosAsync(propertyId, imageForm, videoForm).Returns(expectedResult);

            controller = new ImageController(_imageService);

            // Act
            IActionResult actionResult = await controller.Add(propertyId, imageForm, videoForm);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }


        [Fact]
        public async Task When_Add_BadRequest()
        {
            // Arrange
            PropertyImageForm? imageForm = null;
            PropertyVideoForm? videoForm = new();
            int propertyId = 1;

            controller = new ImageController(_imageService);

            // Act
            IActionResult? actionResult = await controller.Add(propertyId, imageForm, videoForm);
            BadRequestResult? badRequestResult = actionResult as BadRequestResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
        }



        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task When_GetPhotos_Success(int propertyId)
        {
            // Arrange
            int expectedStatusCode = (int)HttpStatusCode.OK;
            _imageService.GetPhotosAsync(propertyId).Returns(new ServiceResult());

            controller = new RealEstateAdmin.Api.Controllers.ImageController(_imageService);

            // Act
            var actionResult = await controller.GetPhotos(propertyId);
            var actualStatusCode = (actionResult as ObjectResult)?.StatusCode;

            // Assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task When_GetPhotos_BadRequest()
        {
            //Arrange
            int id = 0;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _imageService.GetPhotosAsync(id).Returns(result);

            //Act
            controller = new ImageController(_imageService);
            var response = await controller.GetPhotos(id);
            var actualResult = (response as StatusCodeResult).StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }



        [Fact]
        public async Task When_DeletePhotos_Success()
        {
            // Arrange
            int propertyId = 1;
            ServiceResult expectedResult = new ServiceResult();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK; // Set the expected status code
            _imageService.DeletePhotosByPropertyIdAsync(propertyId).Returns(expectedResult);

            controller = new ImageController(_imageService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, "propertyId")
                }, "mock"))
            };

            // Act
            IActionResult actionResult = await controller.DeletePhotos(propertyId);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }



        [Fact]
        public async Task When_DeletePhotoss_Success()
        {
            // Arrange
            int propertyId = 1;
            ServiceResult expectedResult = new ServiceResult();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK; // Set the expected status code
            _imageService.DeletePhotosByPropertyIdAsync(propertyId).Returns(expectedResult);

            controller = new ImageController(_imageService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, "propertyId")
                }, "mock"))
            };

            // Act
            IActionResult actionResult = await controller.DeletePhotos(propertyId);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }




        [Fact]
        public async Task When_DeletePhotos_BadRequest()
        {
            //Arrange
            int id = 0;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _imageService.DeletePhotosByPropertyIdAsync(id).Returns(result);

            //Act
            controller = new ImageController(_imageService);
            var response = await controller.DeletePhotos(id);
            var actualResult = (response as StatusCodeResult).StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }




        [Fact]
        public async Task When_DeletePhotosByPhotoId_Success()
        {
            // Arrange
            int imageId = 1;
            ServiceResult? expectedResult = new ServiceResult();
            _imageService.DeletePhotosByPhotoIdAsync(imageId).Returns(expectedResult);

            controller = new ImageController(_imageService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, "imageId")
            }, "mock"))
            };

            // Act
            IActionResult actionResult = await controller.DeletePhotosByPhotoId(imageId);
            OkObjectResult? okResult = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(expectedResult, okResult?.Value);
        }

        [Fact]
        public async Task When_DeletePhotosByPhotoId_BadRequest()
        {
            //Arrange
            int imageId = 0;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _imageService.DeletePhotosByPhotoIdAsync(imageId).Returns(result);

            //Act
            controller = new ImageController(_imageService);
            var response = await controller.DeletePhotos(imageId);
            var actualResult = (response as StatusCodeResult).StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }







    }
}
