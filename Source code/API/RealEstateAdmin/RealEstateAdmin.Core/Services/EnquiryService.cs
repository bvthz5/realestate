using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;

namespace RealEstateAdmin.Core.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EnquiryService> _logger;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";
        public EnquiryService(IUnitOfWork unitOfWork, ILogger<EnquiryService> logger, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailService = emailService;
        }

        /// <summary>
        /// List Enquiry
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult> ListEnquiry()
        {
            ServiceResult result = new();
            try
            {
                List<Enquiry> enquiry = await _unitOfWork.EnquiryRepository.GetEnquiryList();
                enquiry = enquiry.Where(enquiry => (enquiry.Status == (byte)EnquiryStatus.Pending
                    || enquiry.Status == (byte)EnquiryStatus.Accepted)
                    && enquiry.Property.Status != (byte)PropertyStatus.Deleted)
                    .OrderByDescending(enquiry => enquiry.CreatedOn)
                    .ToList();
                if (enquiry.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No enquiry Found";
                    return result;
                }


                result.ServiceStatus = ServiceStatus.Success;
                result.Data = enquiry.ConvertAll(enquiry => new EnquiryView(enquiry));
                result.Message = $"Success: Listed";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "ES-01 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }


        /// <summary>
        /// Change Equiry Status
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ChangeStatus(int enquiryId, byte status)
        {
            ServiceResult result = new();
            try
            {
                Enquiry? enquiry = await _unitOfWork.EnquiryRepository.FindByIdAsync(enquiryId);
                string? statusValue = Enum.GetName(typeof(EnquiryStatus), status);
                if (enquiry == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Not Found";
                    return result;
                }
                if (enquiry.Status == (byte)EnquiryStatus.Rejected)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Enquiry Is Already Rejected ";
                    return result;
                }
                if (enquiry.Status == (byte)EnquiryStatus.Accepted)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Enquiry Already Approved";
                    return result;
                }
                if (enquiry.Status == status)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Already in " + statusValue + " Status";
                    return result;
                }
                if (status != (byte)EnquiryStatus.Pending && status != (byte)EnquiryStatus.Accepted && status != (byte)EnquiryStatus.Rejected && status != (byte)EnquiryStatus.Completed)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Status";
                    return result;
                }
                enquiry.Status = status;
                enquiry.UpdatedOn = DateTime.Now;
                _unitOfWork.EnquiryRepository.Update(enquiry);
                await _unitOfWork.SaveAsync();
                string name = enquiry.Name;
                string? enquiryType = Enum.GetName(typeof(EnquiryType), status);
                _emailService.ChangeStatusEnquiry(enquiry.Email, statusValue, name, enquiryType);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Enquiry Status Changed Successfully";
                result.Data = new EnquiryView(enquiry);
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "ES-02 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get Enquiry
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> GetEnquiryAsync(int enquiryId)
        {
            ServiceResult result = new();
            try
            {
                var enquiry = (await _unitOfWork.EnquiryRepository.FindByIdAsync(enquiryId));
                if (enquiry == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $" Not Found for Id : {enquiryId}";
                    return result;
                }
                result.Data = new EnquiryView(enquiry);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "ES-03 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }



        /// <summary>
        /// Get Enquiry Count
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult> GetCount()
        {
            ServiceResult result = new();
            try
            {
                List<Enquiry> enquiries = await _unitOfWork.EnquiryRepository.GetEnquiryList();
                enquiries = enquiries.Where(enquiry => (enquiry.Status == (byte)EnquiryStatus.Pending
                    || enquiry.Status == (byte)EnquiryStatus.Accepted)
                    && enquiry.Property.Status != (byte)PropertyStatus.Deleted)
                    .OrderByDescending(enquiry => enquiry.CreatedOn)
                    .ToList();
                if (enquiries.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No Enquiries Found";
                    return result;
                }
                int totalEnquiries = enquiries.Count;
                int tourCount = (enquiries.Count(enquiry => enquiry.EnquiryType == (byte)EnquiryType.RequestTour));
                int rentCount = (enquiries.Count(enquiry => enquiry.EnquiryType == (byte)EnquiryType.RequestRent));
                int buyCount = (enquiries.Count(enquiry => enquiry.EnquiryType == (byte)EnquiryType.RequestBuy));
                var enquiryCounts = new Dictionary<string, int>()
                {
                    { "tourCount", tourCount },
                    { "rentCount", rentCount },
                    { "buyCount", buyCount },
                    {"totalEnquiries" ,totalEnquiries}
                };
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = enquiryCounts;
                result.Message = "Enquiry Counts";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "ES-04 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }
    }
}
