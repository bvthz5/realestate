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
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Test.API.Controller
{
    public class AdminControllerTests
    {
        private readonly IAdminService _adminService = Substitute.For<IAdminService>();
        AdminController? adminController;
        [Fact]
        public async Task When_Login_Success()
        {
            LoginForm? form = new();
            ServiceResult expectedResult = new();
            expectedResult.ServiceStatus = (Core.Enums.ServiceStatus)HttpStatusCode.OK;
            _adminService.Login(form).Returns(expectedResult);
            adminController = new AdminController(_adminService);
            IActionResult actionResult = await adminController.Login(form);
            ObjectResult? result = actionResult as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)expectedResult.ServiceStatus, result?.StatusCode);
            Assert.Equal(expectedResult,result?.Value);
        }

        [Fact]
        public async Task When_Add_BadRequest()
        {
            // Arrange
            LoginForm? form = null;

            adminController = new AdminController(_adminService);

            // Act
            IActionResult? actionResult = await adminController.Login(form);
            BadRequestResult? badRequestResult = actionResult as BadRequestResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
        }



    }
}
