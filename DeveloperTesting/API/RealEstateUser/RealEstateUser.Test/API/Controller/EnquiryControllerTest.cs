using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RealEstateUser.Api.Controllers;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;
using Xunit;

namespace RealEstateUser.Test.API.Controller
{
    public class EnquiryControllerTest
    {


        [Fact]
        public async Task Tour_ReturnsBadRequest_WhenFormIsNull()
        {
            // Arrange
            var controller = new EnquiryController(Mock.Of<IEnquiryService>(), Mock.Of<SecurityUtil>());

            // Act
            var result = await controller.Tour(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Tour_ActionIsDecoratedWithAuthorizeAttribute()
        {
            // Arrange
            var controllerType = typeof(EnquiryController);
            var actionMethodInfo = controllerType.GetMethod(nameof(EnquiryController.Tour));

            // Act
            var authorizeAttribute = actionMethodInfo?.GetCustomAttribute<AuthorizeAttribute>();

            // Assert
            Assert.NotNull(authorizeAttribute);
        }





    }
}
