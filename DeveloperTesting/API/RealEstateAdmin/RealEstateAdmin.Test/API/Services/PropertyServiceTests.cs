using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Test.API.Services
{
    public class PropertyServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<PropertyService>> _loggerMock;
        private readonly Mock<IPropertyAdditionalInfoService> _propertyAdditionalInfoServiceMock;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyService _propertyService;

        public PropertyServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<PropertyService>>();
            _propertyAdditionalInfoServiceMock = new Mock<IPropertyAdditionalInfoService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _propertyService = new PropertyService(_unitOfWorkMock.Object, _loggerMock.Object, _propertyAdditionalInfoServiceMock.Object);
        }

        [Fact]
        public async Task AddProperty_RecordAlreadyExists_ReturnsRecordAlreadyExists()
        {
            // Arrange
            var form = new PropertyForm { Address = "123 Main St", Latitude = 1.23, Longitude = 4.56 };
            var properties = new List<Property> { new Property { Address = form.Address, Latitude = form.Latitude, Longitude = form.Longitude } };
            _unitOfWorkMock.Setup(uow => uow.PropertyRepository.Find(It.IsAny<Expression<Func<Property, bool>>>())).ReturnsAsync(properties);

            // Act
            var result = await _propertyService.AddProperty(form);

            // Assert
            Assert.Equal(ServiceStatus.RecordAlreadyExists, result.ServiceStatus);
            Assert.Equal("Property Already Exists", result.Message);
        }

        [Fact]
        public async Task AddProperty_InvalidCategoryId_ReturnsInvalidRequest()
        {
            // Arrange
            var form = new PropertyForm { Address = "123 Main St", CategoryId = 1 };
            _unitOfWorkMock.Setup(uow => uow.PropertyRepository.Find(It.IsAny<Expression<Func<Property, bool>>>())).ReturnsAsync(new List<Property>());
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.FindByIdAndStatusAsync(form.CategoryId, (byte)CategoryStatus.Active)).ReturnsAsync((Category)null);

            // Act
            var result = await _propertyService.AddProperty(form);

            // Assert
            Assert.Equal(ServiceStatus.InvalidRequest, result.ServiceStatus);
            Assert.Equal("Invalid Category Id", result.Message);
        }

        

    } }
