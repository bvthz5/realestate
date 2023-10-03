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
    public class GoogleUserControllerTests
    {
        private readonly IGoogleService _googleService = Substitute.For<IGoogleService>();

        GoogleUserController? controller;

        
        // Test case for a successfull login

        [Fact]
        public async Task Login_ValidIdToken_ReturnsOk()
        {
            // Arrange
            var expectedResult = new ServiceResult { ServiceStatus = ServiceStatus.Success };
            _googleService.Login("valid_id_token").Returns(expectedResult);
            controller = new GoogleUserController(_googleService);

            // Act
            var response = await controller.Login("valid_id_token");
            var actualResult = response as ObjectResult;
            var actualStatusCode = actualResult?.StatusCode;
            var actualServiceResult = actualResult?.Value as ServiceResult;

            // Assert
            Assert.NotNull(actualResult);
            Assert.Equal((int)HttpStatusCode.OK, actualStatusCode);
            Assert.NotNull(actualServiceResult);
            Assert.Equal(ServiceStatus.Success, actualServiceResult!.ServiceStatus);
        }

        // -------------------------------------------------------------------TEST------------------------------------------------------------------------------------------------------------ //
    }
}
