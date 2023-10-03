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
    public class UserControllerTests
    {
        private readonly IUserService _userService = Substitute.For<IUserService>();
        UserController? userController;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task When_GetUser_Succes(int userId)
        {

            //Arrange
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult result = new();

            _userService.GetUserAsync(userId).Returns(result);

            //Act
            userController = new RealEstateAdmin.Api.Controllers.UserController(_userService);
            var response = await userController.GetUser(userId);
            var actualResult = response?.Result as ObjectResult;
            //Assert
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }

        [Fact]
        public async Task When_GetUser_BadRequest()
        {
            //Arrange
            int userId = 0;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _userService.GetUserAsync(userId).Returns(result);

            //Act
            userController = new UserController(_userService);
            var response = await userController.GetUser(userId);
            var actualResult = (response?.Result as StatusCodeResult)?.StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }



        [Theory]
        [InlineData("query1", "name", true)]
        [InlineData("query2", "date", false)]
        public async Task When_UsersList_Succes(string? searchQuery, string? sortBy, bool isSortAscending)
        {

            //Arrange
            int expectedResult = (int)HttpStatusCode.OK;
            ServiceResult result = new();

            _userService.GetUserList(searchQuery, sortBy, isSortAscending).Returns(result);

            //Act
            userController = new RealEstateAdmin.Api.Controllers.UserController(_userService);
            var response = await userController.UsersList(searchQuery, sortBy, isSortAscending);
            var actualResult = response as ObjectResult;
            //Assert
            Assert.Equal(expectedResult, actualResult?.StatusCode);
        }

        [Fact]
        public async Task When_ChangeStatusAsync_Success()
        {
            // Arrange
            int userId = 1;
            byte status = 1;
            ServiceResult expectedResult = new ServiceResult();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK;
            _userService.ChangeStatusAsync(userId, status).Returns(expectedResult);

            userController = new UserController(_userService);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "userId")
                }, "mock"))
            };

            // Act
            IActionResult actionResult = await userController.ChangeUserStatus(userId, status);
            ObjectResult? result = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult, result?.Value);
        }


        [Fact]
        public async Task When_ChangeStatusAsync_BadRequest()
        {
            //Arrange
            int userId = 0;
            byte status = 1;
            int expectedResult = (int)HttpStatusCode.BadRequest;
            ServiceResult? result = new()
            {
                Data = 123,
                Message = "test"
            };
            _userService.GetUserAsync(userId).Returns(result);

            //Act
            userController = new UserController(_userService);
            var response = await userController.ChangeUserStatus(userId, status);
            var actualResult = (response as StatusCodeResult)?.StatusCode;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }


    }
}
