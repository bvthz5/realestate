using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;

namespace RealEstateAdmin.Test.API.Services
{
    public class EnquiryServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<EnquiryService>> _loggerMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly EnquiryService _enquiryService;

        public EnquiryServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<EnquiryService>>();
            _emailServiceMock = new Mock<IEmailService>();
            _enquiryService = new EnquiryService(_unitOfWorkMock.Object, _loggerMock.Object, _emailServiceMock.Object);
        }


        [Fact]
        public async Task When_ListEnquiry_Success()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILogger<EnquiryService>>();
            var mockEmailService = new Mock<IEmailService>();
            // Arrange
            var enquiryList = new List<Enquiry>()

        {
            new Enquiry
            {
                EnquiryId = 1,
                UserId = 1,
                PropertyId = 1,
                Name = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "1234567890",
                Message = "I am interested in the property.",
                EnquiryType = (byte)EnquiryType.RequestBuy,
                AvailableDates = DateTime.Now.AddDays(7),
                AvailableTime = "2:00 PM - 4:00 PM",
                Status = (byte)EnquiryStatus.Pending,
                CreatedOn = DateTime.Now.AddDays(-1),
                Property = new Property
                {
                    PropertyId = 1,
                    CategoryId = 1,
                    Address = "123 Main St.",
                    Description = "A beautiful property.",
                    ZipCode = "12345",
                    Price = 100000,
                    Status = (byte)PropertyStatus.Active,
                    City = "New York",
                    Category = new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Residential"
                    }
                }
            }
        };
            _unitOfWorkMock.Setup(uow => uow.EnquiryRepository.GetEnquiryList())
                .ReturnsAsync(enquiryList);

            // Act
            var result = await _enquiryService.ListEnquiry();


            // Assert
            Assert.Equal(ServiceStatus.Success, result.ServiceStatus);
            Assert.NotNull(result.Data);
            Assert.IsType<List<EnquiryView>>(result.Data);
            Assert.Equal(1, ((List<EnquiryView>?)result.Data)?.Count);
            var enquiryView = ((List<EnquiryView>?)result.Data)?[0];
            Assert.Equal(1, enquiryView?.EnquiryId);
            Assert.Equal(1, enquiryView?.UserId);
            Assert.Equal(1, enquiryView?.PropertyId);
            Assert.Equal("John Doe", enquiryView?.Name);
            Assert.Equal("johndoe@example.com", enquiryView?.Email);
            Assert.Equal("1234567890", enquiryView?.PhoneNumber);
            Assert.Equal("I am interested in the property.", enquiryView?.Message);
            Assert.Equal(EnquiryType.RequestBuy.ToString(), enquiryView?.EnquiryType);
            Assert.Equal(DateTime.Now.AddDays(7).Date, enquiryView?.AvailableDates?.Date);
            Assert.Equal("2:00 PM - 4:00 PM", enquiryView?.AvailableTime);
            Assert.Equal((byte)EnquiryStatus.Pending, enquiryView?.Status);
            Assert.Equal("123 Main St.", enquiryView?.Property);
            Assert.Equal("Residential", enquiryView?.CategoryType);
            Assert.Equal("A beautiful property.", enquiryView?.Description);
            Assert.Equal("12345", enquiryView?.Zipcode);
            Assert.Equal(100000, enquiryView?.Price);
            Assert.Equal("New York", enquiryView?.City);

        }


        [Fact]
        public async Task When_ListEnquiry_ReturnsNoRecordFoundResult()
        {
            // Arrange
            var enquiries = new List<Enquiry>();
            _unitOfWorkMock.Setup(uow => uow.EnquiryRepository.GetEnquiryList()).ReturnsAsync(enquiries);

            // Act
            var result = await _enquiryService.ListEnquiry();

            // Assert
            Assert.Equal(ServiceStatus.NoRecordFound, result.ServiceStatus);
            Assert.Equal("No enquiry Found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task When_ListEnquiry_ReturnsServerErrorResult()
        {
            // Arrange
            _unitOfWorkMock.Setup(uow => uow.EnquiryRepository.GetEnquiryList()).ThrowsAsync(new Exception());

            // Act
            var result = await _enquiryService.ListEnquiry();

            // Assert
            Assert.Equal(ServiceStatus.ServerError, result.ServiceStatus);
            Assert.Equal("ES-01 : Server Error", result.Message);
            Assert.Null(result.Data);
        }




        [Fact]
        public async Task ChangeStatus_EnquiryNotFound_ReturnsNoRecordFound()
        {
            // Arrange
            int enquiryId = 111;
            byte status = (byte)EnquiryStatus.Pending;
            Enquiry? enquiry = null;
            _unitOfWorkMock.Setup(uow => uow.EnquiryRepository.FindByIdAsync(enquiryId)).ReturnsAsync(enquiry);

            // Act
            ServiceResult result = await _enquiryService.ChangeStatus(enquiryId, status);

            // Assert
            Assert.Equal(ServiceStatus.NoRecordFound, result.ServiceStatus);
            Assert.Equal("Not Found", result.Message);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
            _emailServiceMock.Verify(es => es.ChangeStatusEnquiry(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }



    }

}
