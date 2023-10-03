using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.DTO.Views;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Core.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<EnquiryService> _logger;

        private readonly SecurityUtil _securityUtil;

        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public EnquiryService(IUnitOfWork unitOfWork, ILogger<EnquiryService> logger, SecurityUtil securityUtil)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _securityUtil = securityUtil;
        }

        public async Task<ServiceResult> RequestTour(EnquiryTourForm form, int userId)
        {
            ServiceResult result = new();
            try
            {
                User? user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User not found";
                    return result;
                }

                switch ((UserStatus)user.Status)
                {
                    case UserStatus.Blocked:
                    case UserStatus.Deleted:
                    case UserStatus.Inactive:
                        result.ServiceStatus = ServiceStatus.NoRecordFound;
                        result.Message = $"User is {nameof(UserStatus)}.{(UserStatus)user.Status}";
                        return result;
                }

                Property? properties = await _unitOfWork.PropertyRepository.FindByIdAsync(form.PropertyId);

                if (properties == null || properties.Status == ((byte)PropertyStatus.Deleted) || _securityUtil.GetCurrentUserId() == 0 && properties.Status != (byte)PropertyStatus.Active)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";
                    return result;
                }

                if (properties.Status == ((byte)PropertyStatus.SoldOut) || properties.Status == ((byte)PropertyStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = $"Property Is : {(PropertyStatus)properties.Status}";
                    return result;
                }

                Enquiry enquiry = new()
                {
                    PropertyId = form.PropertyId,
                    UserId = userId,
                    EnquiryType = ((byte)((EnquiryType)form.EnquiryType)),
                    AvailableDates = form.AvailableDates,
                    AvailableTime = form.AvailableTime,
                    Name = form.Name,
                    Email = user.Email,
                    PhoneNumber = form.Phone,
                    Message = form.Message,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    CreatedBy = user.UserId,
                    Status = (byte)EnquiryStatus.Pending,

                };

                if (form.EnquiryType == (byte)EnquiryType.RequestBuy && properties.Category.Type != (byte)CategoryType.Buy)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = $"EnquiryType is not valid for this {nameof(CategoryType)}.{properties.Category.Type} property";
                    return result;
                }
                if (form.EnquiryType == (byte)EnquiryType.RequestRent && properties.Category.Type != (byte)CategoryType.Rent)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = $"EnquiryType is not valid for this {nameof(CategoryType)}.{properties.Category.Type} property";
                    return result;
                }

                if (await EnquiryChecker(userId, form.PropertyId, form.EnquiryType))

                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Tour already requested";
                    return result;
                }

                if (enquiry.EnquiryType != (byte)EnquiryType.RequestBuy && enquiry.EnquiryType != (byte)EnquiryType.RequestTour && enquiry.EnquiryType != (byte)EnquiryType.RequestRent)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "EnquiryType is not found";
                    return result;
                }

                enquiry = await _unitOfWork.EnquiryRepository.Add(enquiry);

                await _unitOfWork.SaveAsync();

                result.Data = new EnquiryTourView(enquiry);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Approve Request send for Tour Request";
                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "EnS=01:ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;

        }

        public async Task<bool> EnquiryChecker(int userId, int propertyId, int enquiryType)
        {
            List<Enquiry> enquiryList = (await _unitOfWork.EnquiryRepository.Find(enq => enq.UserId == userId && enq.PropertyId == propertyId)).ToList();

            return (enquiryList.Any(enq => enq.EnquiryType == enquiryType && (enq.Status == (byte)EnquiryStatus.Pending || enq.Status == (byte)EnquiryStatus.Accepted)));

        }

        public async Task<ServiceResult> EnquiryList(EnquiryPaginationForm form, int userId)
        {
            ServiceResult result = new();
            try
            {
                User? user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User Not Found with Id : {userId}";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }

                List<Enquiry>? enquiry;
                if (form.SortBy != null && !_unitOfWork.EnquiryRepository.ColumnMapForSortBy.ContainsKey(form.SortBy))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = $"SortBy : Accepts [{string.Join(", ", _unitOfWork.EnquiryRepository.ColumnMapForSortBy.Keys)}] values only";

                    return result;
                }

                enquiry = await _unitOfWork.EnquiryRepository.GetEnquiryList();
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
                byte[] Status = form.Status.Split(',')
                                           .Select(selector => byte
                                           .TryParse(selector, out byte x) ? x : (byte)255)
                                           .Where(x => x != 255)
                                           .ToArray();

                enquiry = await _unitOfWork.EnquiryRepository.FindAllByCategoryType(

                                                form.Search,
                                                form.SortBy,
                                                form.SortByDesc,
                                                form.EnquiryType,
                                                Status
                                                );
                int currentUserId = _securityUtil.GetCurrentUserId();

                enquiry = enquiry.Where(enquiry =>
                                                enquiry.CreatedBy == currentUserId &&
                                                enquiry.Status == ((byte)((EnquiryStatus)enquiry.Status))
                                          ).ToList();

                List<RequestDetailview> enquiryViews = enquiry.ConvertAll(product => new RequestDetailview(product)).ToList();

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = new Pager<RequestDetailview>(form.PageNumber, form.PageSize, enquiryViews);

                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS=02:ServerError";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> GetEnquiryAsync(int enquiryId)
        {
            ServiceResult result = new();
            try
            {
                var enquiry = (await _unitOfWork.EnquiryRepository.FindByIdAsync(enquiryId));

                if (enquiry == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"Enquiry Not Found for Id : {enquiryId}";
                    return result;
                }

                result.Data = new RequestDetailview(enquiry);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                return result;

            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = $"Error :{e.Message}";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }
    }
}
