using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Api.Controllers;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;
using RealEstateUser.Core.Services;
using Xunit;

namespace RealEstateUser.Test.API.Controller
{
    public class UserControllerTests
    {
        private readonly IUserService _userService;
        private readonly SecurityUtil _securityUtil;
        UserController? _controller;

        public UserControllerTests()
        {
            _userService = Substitute.For<IUserService>();
            _securityUtil = Substitute.For<SecurityUtil>();
            _controller = new UserController(_userService, _securityUtil);
        }
        //REGISTER

        [Fact]
        public async Task Register_ValidForm_ReturnsOK()
        {
            var form = new UserRegistrationForm()
            {
                Email = "binilvincent80@gmail.com",
                Password = "P@sswr3d@"

            };
            var expectedResult = HttpStatusCode.OK;
            var result = new ServiceResult { ServiceStatus = ServiceStatus.Success };

            _userService.AddUser(form).Returns(result);
            _controller = new UserController(_userService, securityUtil: null);

            // Act
            var response = await _controller.Register(form);
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

        [Fact]
        public async Task Register_InvalidForm_ReturnsBadRequest()
        {
            // Arrange
            UserRegistrationForm? form = null;

            // Act
            IActionResult? result = await _controller.Register(form);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        // -------------------------------------------------------------------TEST-2---------------------------------------------------------------------------------------------------------- //

        //EMAIL_VERIFICATION

        [Fact]
        public async Task EmailVerification_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = "valid_token";
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.VerifyUser(token)).ReturnsAsync(new ServiceResult
            {
                ServiceStatus = ServiceStatus.Success,
                Data = null,
                Message = "User email has been verified successfully."
            });
            var controller = new UserController(userServiceMock.Object, null);

            // Act
            var result = await controller.EmailVerification(token);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, (result as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task EmailVerification_NullToken_ReturnsBadRequest()
        {
            // Arrange
            string nullToken = null;
            var controller = new UserController(Mock.Of<IUserService>(), null);

            // Act
            var response = await controller.EmailVerification(nullToken);

            // Assert
            var result = Assert.IsType<BadRequestResult>(response);
        }

        // -------------------------------------------------------------------TEST-3---------------------------------------------------------------------------------------------------------- //

        // FORGOT PASSWORD

        // Test for a valid email that should return OK

        [Fact]
        public async Task Forgot_Password_Valid_Email_ReturnOK()
        {
            // Arrange
            var email = "test@example.com";
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.ForgotPasswordRequest(email)).ReturnsAsync(new ServiceResult
            {
                ServiceStatus = ServiceStatus.Success,
                Data = null,
                Message = "User email has been verified successfully."
            });
            var controller = new UserController(userServiceMock.Object, null);

            // Act
            var result = await controller.ForgotPassword(email);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, (result as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task Forgot_Password_Invalid_Email_ReturnBadRequest()
        {
            // Arrange
            var email = "realestate5bvgmail.com";
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.ForgotPasswordRequest(email)).ReturnsAsync(new ServiceResult
            {
                ServiceStatus = ServiceStatus.InvalidRequest,
                Data = null,
                Message = "Not a Valid Email Address"
            });
            var controller = new UserController(userServiceMock.Object, null);

            // Act
            var result = await controller.ForgotPassword(email);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, (result as ObjectResult).StatusCode);
        }



        [Fact]
        public async Task Forgot_Password_Null_Email_ReturnBadRequest()
        {
            // Arrange

            // Act
            var result = await _controller.ForgotPassword(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            var badRequestResult = result as BadRequestResult;
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult?.StatusCode);
        }

        [Fact]
        public async Task Forgot_Password_Empty_Email_ReturnBadRequest()
        {
            // Arrange

            // Act
            var result = await _controller.ForgotPassword(string.Empty);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            var badRequestResult = result as BadRequestResult;
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult?.StatusCode);
        }

        [Fact]
        public async Task Forgot_Password_InValid_Email__ReturnsBadRequest()
        {
            // Arrange
            string? email = null;
            var controller = new UserController(Mock.Of<IUserService>(), null);

            // Act
            var response = await controller.ForgotPassword(email);

            // Assert
            var result = Assert.IsType<BadRequestResult>(response);
        }

        // -------------------------------------------------------------------TEST-4---------------------------------------------------------------------------------------------------------- //

        //RESET PASSWORD

        [Fact]
        public async Task Reset_Password_Valid_Form_ReturnOK()
        {
            // Arrange
            var form = new ForgotPasswordForm
            {
                Token = "test-token",
                Password = "Tst.P@sswr3d",
                ConfirmPassword = "Tst.P@sswr3d"
            };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.ResetPassword(form)).ReturnsAsync(new ServiceResult
            {
                ServiceStatus = ServiceStatus.Success,
                Data = null,
                Message = "Password reset successful"
            });
            _controller = new UserController(userServiceMock.Object, null);

            // Act
            var result = await _controller.ResetPassword(form);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, (result as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task Reset_Password_Null_Form_ReturnsBadRequest()
        {
            // Arrange
            ForgotPasswordForm form = null;
            _controller = new UserController(Mock.Of<IUserService>(), null);

            // Act
            var result = await _controller.ResetPassword(form);

            // Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task Reset_Password_Service_Error_ReturnInternalServerError()
        {
            // Arrange
            var form = new ForgotPasswordForm
            {
                Token = "test-token",
                Password = "Tst.P@sswr3d",
                ConfirmPassword = "Tst.P@sswr3d"
            };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.ResetPassword(form)).ReturnsAsync(new ServiceResult
            {
                ServiceStatus = ServiceStatus.ServerError,
                Data = null,
                Message = "An error occurred while resetting password"
            });
            _controller = new UserController(userServiceMock.Object, null);

            // Act
            var result = await _controller.ResetPassword(form);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, (result as ObjectResult).StatusCode);
        }

        // -------------------------------------------------------------------TEST-5---------------------------------------------------------------------------------------------------------- //


        //PROFILE PIC

        [Fact]
        public async Task SetProfilePic_NullImage_ReturnsBadRequest()
        {
            // Arrange
            _controller = new UserController(Mock.Of<IUserService>(), Mock.Of<SecurityUtil>());
            ImageForm form = null;

            // Act
            var result = await _controller.SetProfilePic(form);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        //  valid image


        // -------------------------------------------------------------------TEST-6---------------------------------------------------------------------------------------------------------- //

        //EDIT PROFILE

        [Fact]
        public async Task Update_NullForm_ReturnsBadRequest()
        {
            // Arrange
            UserUpdateForm? form = null;
            _controller = new UserController(_userService, new SecurityUtil());

            // Act
            var result = await _controller.Update(form);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }


    }
}


