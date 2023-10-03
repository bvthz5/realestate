using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RealEstateAdmin.Api.Controllers;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;
using System.Net;
using System.Security.Claims;

namespace RealEstateAdmin.Test.API.Controller
{
    public class EnquiryControllerTests
    {
        private readonly IEnquiryService _enquiryService = Substitute.For<IEnquiryService>();
        EnquiryController? controller;
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task When_GetEnquiryAsync_Succes(int id)
        {

            //Arrange
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult result = new();

            _enquiryService.GetEnquiryAsync(id).Returns(result);

            //Act
            controller = new RealEstateAdmin.Api.Controllers.EnquiryController(_enquiryService);
            var response = await controller.GetEnquiry(id);
            var actualResult = response?.Result as ObjectResult;
            //Assert
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }

        [Fact]
        public async Task When_GetEnquiryAsync_BadRequest()
        {
            //Arrange
            int id = 0;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _enquiryService.GetEnquiryAsync(id).Returns(result);

            //Act
            controller = new EnquiryController(_enquiryService);
            var response = await controller.GetEnquiry(id);
            var actualResult = (response?.Result as StatusCodeResult)?.StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public async Task When_List_Success()
        {
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult? result = new();
            _enquiryService.ListEnquiry().Returns(result);
            controller = new EnquiryController(_enquiryService);
            var response = await controller.List();
            var actualResult = response as ObjectResult;
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }



        [Fact]
        public async Task When_List_BadRequest()
        {
            // Arrange
            controller = new EnquiryController(_enquiryService);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            IActionResult actionResult = await controller.List();
            BadRequestObjectResult? badRequestResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.IsType<SerializableError>(badRequestResult?.Value);
        }




        [Fact]
        public async Task When_ChangeStatus_Success()
        {
            // Arrange
            int enquiryId = 1;
            byte status = 1;
            ServiceResult expectedResult = new ServiceResult();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK;
            _enquiryService.ChangeStatus(enquiryId, status).Returns(expectedResult);

            controller = new EnquiryController(_enquiryService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "enquiryId")
                }, "mock"))
            };

            // Act
            IActionResult actionResult = await controller.ChangeStatus(enquiryId, status);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }

        [Fact]
        public async Task When_Enquiries_Success()
        {
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult result = new();
            _enquiryService.GetCount().Returns(result);
            controller = new EnquiryController(_enquiryService);
            var response = await controller.Enquiries();
            var actualResult = response as ObjectResult;
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }


        [Fact]
        public async Task When_Enquiries_BadRequest()
        {
            // Arrange
            controller = new EnquiryController(_enquiryService);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            IActionResult actionResult = await controller.Enquiries();
            BadRequestObjectResult? badRequestResult = actionResult as BadRequestObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.IsType<SerializableError>(badRequestResult?.Value);
        }
    }
}
