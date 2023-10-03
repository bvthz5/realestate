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
    public class PropertyControllerTests
    {
        private readonly IPropertyService _propertyService = Substitute.For<IPropertyService>();

        PropertyController? controller;

        // Get a property by ID

        [Fact]
        public async Task GetProperty_ValidId_ReturnsOk()
        {
            // Arrange
            int propertyId = 1;
            var expectedResult = new ServiceResult { ServiceStatus = ServiceStatus.Success };
            _propertyService.GetPropertyAsync(propertyId).Returns(expectedResult);
            controller = new PropertyController(_propertyService);

            // Act
            var response = await controller.Getproperty(propertyId);
            var actualResult = response as ObjectResult;
            var actualStatusCode = actualResult?.StatusCode;
            var actualServiceResult = actualResult?.Value as ServiceResult;

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal((int)HttpStatusCode.OK, actualStatusCode);
            Assert.NotNull(actualServiceResult);
            Assert.Equal(ServiceStatus.Success, actualServiceResult!.ServiceStatus);
        }

        [Fact]
        public async Task GetProperty_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int propertyId = 0;
            controller = new PropertyController(_propertyService);

            // Act
            var response = await controller.Getproperty(propertyId);
            var actualResult = response as BadRequestResult;

            // Assert
            Assert.NotNull(actualResult);
        }

        // Get a paginated list of property

        [Fact]
        public async Task PaginatedPropertyList_ValidForm_ReturnsOk()
        {
            // Arrange
            var form = new PropertyPaginationForm { PageNumber = 1, PageSize = 10 };
            var expectedResult = new ServiceResult { ServiceStatus = ServiceStatus.Success };
            _propertyService.PropertyListAsync(form).Returns(expectedResult);
            controller = new PropertyController(_propertyService);

            // Act
            var response = await controller.PaginatedPropertyList(form);
            var actualResult = response as ObjectResult;
            var actualStatusCode = actualResult?.StatusCode;
            var actualServiceResult = actualResult?.Value as ServiceResult;

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal((int)HttpStatusCode.OK, actualStatusCode);
            Assert.NotNull(actualServiceResult);
            Assert.Equal(ServiceStatus.Success, actualServiceResult!.ServiceStatus);
        }

        [Fact]
        public async Task PaginatedPropertyList_NullForm_ReturnsBadRequest()
        {
            // Arrange
            PropertyPaginationForm form = null;
            controller = new PropertyController(_propertyService);

            // Act
            var response = await controller.PaginatedPropertyList(form);
            var actualResult = response as BadRequestResult;

            // Assert
            Assert.NotNull(actualResult);
        }
    }

}

