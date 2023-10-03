using System.Net;
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
    public class CategoryControllerTests
    {
        private readonly ICategoryService _categoryService = Substitute.For<ICategoryService>();

        CategoryController? controller;

        // Test case for a successfull list

        [Fact]
        public async Task GetCategory_ModelStateIsValid_ReturnsOk()
        {
            // Arrange
            var expectedResult = new ServiceResult { ServiceStatus = ServiceStatus.Success };
            _categoryService.CategoryList().Returns(expectedResult);
            controller = new CategoryController(_categoryService);

            // Act
            var response = await controller.GetCategory();
            var actualResult = response as ObjectResult;
            var actualStatusCode = actualResult?.StatusCode;
            var actualServiceResult = actualResult?.Value as ServiceResult;

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal((int)HttpStatusCode.OK, actualStatusCode);
            Assert.NotNull(actualServiceResult);
            Assert.Equal(ServiceStatus.Success, actualServiceResult!.ServiceStatus);
        }

        // Test case for a bad request

        [Fact]
        public async Task GetCategory_ModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            controller = new CategoryController(_categoryService);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var response = await controller.GetCategory();
            var actualResult = response as BadRequestResult;

            // Assert
            Assert.NotNull(actualResult);
        }

        // -------------------------------------------------------------------TEST------------------------------------------------------------------------------------------------------------ //

    }
}
