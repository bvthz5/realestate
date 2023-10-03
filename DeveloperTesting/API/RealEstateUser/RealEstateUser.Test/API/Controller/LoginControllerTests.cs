using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RealEstateUser.Api.Controllers;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;
using Xunit;

namespace RealEstateUser.Test.API.Controller
{
    //LOGIN
    public class LoginControllerTests
    {
        private readonly IUserService _userService = Substitute.For<IUserService>();

        LoginController? controller;

        // Test case for a successfull login

        [Fact]
        public async Task Login_Success()
        {
            // Arrange
            var form = new LoginForm
            {
                Email = "binilvincent80@gmail.com",
                Password = "P@sswr3d@"

            };
            var expectedResult = HttpStatusCode.OK;
            var result = new ServiceResult { ServiceStatus = ServiceStatus.Success };

            _userService.Login(form).Returns(result);
            controller = new LoginController(_userService);

            // Act
            var response = await controller.Login(form);
            var actualResult = response as ObjectResult;
            var actualStatusCode = actualResult?.StatusCode;
            var actualServiceResult = actualResult?.Value as ServiceResult;

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotNull(actualStatusCode);
            Assert.Equal(expectedResult, (HttpStatusCode)actualStatusCode!);
            Assert.NotNull(actualServiceResult);
            Assert.Equal(ServiceStatus.Success, actualServiceResult!.ServiceStatus);
        }


        // Test case for a bad request when form is null

        [Fact]
        public async Task Login_BadRequest_NullForm()
        {
            // Arrange
            LoginForm? form = null;
            controller = new LoginController(_userService);

            // Act
            var response = await controller.Login(form);
            var actualResult = response as BadRequestResult;

            // Assert
            Assert.NotNull(actualResult);
        }

        // -------------------------------------------------------------------TEST-2---------------------------------------------------------------------------------------------------------- //

        //REFRESH TOKEN

        // Test case for a successfull Renewed Access Token Using Refresh Token

        [Fact]
        public async Task Refresh_ValidToken_ReturnsOk()
        {
            // Arrange
            var refreshToken = "valid_token";
            var expectedResult = new ServiceResult { ServiceStatus = ServiceStatus.Success };
            _userService.Refresh(refreshToken).Returns(expectedResult);
            controller = new LoginController(_userService);

            // Act
            var response = await controller.Refresh(refreshToken);
            var actualResult = response as ObjectResult;
            var actualStatusCode = actualResult?.StatusCode ?? 0;
            var actualServiceResult = actualResult?.Value as ServiceResult;

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)actualStatusCode);
            Assert.NotNull(actualServiceResult);
            Assert.Equal(ServiceStatus.Success, actualServiceResult!.ServiceStatus);
        }


        // Test case for a bad request when token is null

        [Fact]
        public async Task Refresh_NullToken_ReturnsBadRequest()
        {
            // Arrange
            string? refreshToken = null;
            controller = new LoginController(_userService);

            // Act
            var response = await controller.Refresh(refreshToken);
            var actualResult = response as BadRequestResult;

            // Assert
            Assert.NotNull(actualResult);
        }

    }
}


