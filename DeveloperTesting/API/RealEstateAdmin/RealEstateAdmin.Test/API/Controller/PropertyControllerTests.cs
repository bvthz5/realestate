using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RealEstateAdmin.Api.Controllers;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using System.Net;
using System.Security.Claims;

namespace RealEstateAdmin.Test.API.Controller
{
    public class PropertyControllerTests
    {
        private readonly IPropertyService _propertyService = Substitute.For<IPropertyService>();
        PropertyController? controller;

        [Fact]
        public async Task When_Add_Success()
        {
            // Arrange
            PropertyForm form = new();
            ServiceResult expectedResult = new();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK; // Set the expected status code
            _propertyService.AddProperty(form).Returns(expectedResult);

            controller = new PropertyController(_propertyService);

            // Act
            IActionResult actionResult = await controller.Add(form);
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
            PropertyForm? form = null;

            controller = new PropertyController(_propertyService);

            // Act
            IActionResult? actionResult = await controller.Add(form);
            BadRequestResult? badRequestResult = actionResult as BadRequestResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
        }



        [Fact]
        public async Task When_ChangeStatus_Success()
        {
            // Arrange
            int propertyId = 1;
            byte status = 1;
            ServiceResult expectedResult = new ServiceResult();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK;
            _propertyService.ChangeStatusAsync(propertyId, status).Returns(expectedResult);

            controller = new PropertyController(_propertyService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "propertyId")
                }, "mock"))
            };

            // Act
            IActionResult actionResult = await controller.ChangePropertyStatus(propertyId, status);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task When_GetPropertyAsync_Succes(int propertyid)
        {

            //Arrange
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult result = new()
            { };
            _propertyService.GetPropertyAsync(propertyid).Returns(result);

            //Act
            controller = new RealEstateAdmin.Api.Controllers.PropertyController(_propertyService);
            var response = await controller.GetProperty(propertyid);
            var actualResult = response as ObjectResult;
            //Assert
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }

        [Fact]
        public async Task When_GetPropertyAsync_BadRequest()
        {
            //Arrange
            int propertyid = 0;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _propertyService.GetPropertyAsync(propertyid).Returns(result);

            //Act
            controller = new PropertyController(_propertyService);
            var response = await controller.GetProperty(propertyid);
            var actualResult = (response as StatusCodeResult).StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task When_EditProperty_Success()
        {
            // Arrange
            PropertyForm form = new();
            int propertyId = 1;
            ServiceResult expectedResult = new();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK; // Set the expected status code
            _propertyService.EditProperty(propertyId, form).Returns(expectedResult);

            controller = new PropertyController(_propertyService);

            // Act
            IActionResult actionResult = await controller.EditProperty(propertyId, form);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }

        /*[Fact]
        public async Task When_EditPropertyy_Success()
        {
            // Arrange
            PropertyForm form = new();
            int propertyId = 1;
            ServiceResult expectedResult = new ServiceResult();
            _propertyService.EditProperty(propertyId, form).Returns(expectedResult);

            controller = new PropertyController(_propertyService);

            // Act
            IActionResult actionResult = await controller.EditProperty(propertyId, form);
            OkObjectResult? okResult = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(expectedResult, okResult?.Value);
        }*/

        [Fact]
        public async Task When_Property_Success()
        {
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult? result = new();
            _propertyService.Count().Returns(result);
            controller = new PropertyController(_propertyService);
            var response = await controller.Property();
            var actualResult = response as ObjectResult;
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }



        [Fact]
        public async Task When_Property_BadRequest()
        {
            // Arrange
            controller = new PropertyController(_propertyService);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            IActionResult actionResult = await controller.Property();
            BadRequestObjectResult? badRequestResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.IsType<SerializableError>(badRequestResult?.Value);
        }
    }
}
