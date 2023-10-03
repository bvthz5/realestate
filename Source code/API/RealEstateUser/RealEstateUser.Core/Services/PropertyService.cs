using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.Options;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.DTO.Views;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Core.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<PropertyService> _logger;
        private readonly SecurityUtil _securityUtil;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";
        public PropertyService(IUnitOfWork unitOfWork, ILogger<PropertyService> logger, SecurityUtil securityUtil)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _securityUtil = securityUtil;
        }

        public async Task<ServiceResult> GetPropertyAsync(int propertyId)
        {
            ServiceResult result = new();
            try
            {
                Property? properties = await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId);

                if (properties == null || _securityUtil.GetCurrentUserId() == 0 && properties.Status != (byte)PropertyStatus.Active)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";
                    return result;
                }
                if (properties.Status == (byte)PropertyStatus.Deleted)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"Property is {(PropertyStatus)((sbyte)properties.Status)}";
                    return result;
                }
                result.Data = new PropertyAdditionalInfoView((await _unitOfWork.PropertyadditionalInfoRepository
                    .FindByPropertyIdAsync(propertyId)), properties, _unitOfWork.PropertyRepository.FindThumbnailPicture(properties.PropertyId)?.PropertyImages);

                result.ServiceStatus = ServiceStatus.Success;
                return result;

            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS=01:ServerError";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> PropertyListAsync(PropertyPaginationForm form)
        {
            ServiceResult result = new();
            try
            {
                List<Property>? properties;

                properties = (await _unitOfWork.PropertyRepository.Find(properties =>
                properties.Status != (byte)PropertyStatus.Deleted &&
                properties.Status != (byte)PropertyStatus.Inactive &&
                properties.Status != (byte)PropertyStatus.Occupied &&
                properties.Status != ((byte)PropertyStatus.SoldOut)))
                .ToList();

                if (properties.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No Property Found";
                    return result;
                }

                if (form.SortBy != null && !_unitOfWork.PropertyRepository.ColumnMapForSortBy.ContainsKey(form.SortBy))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = $"SortBy : Accepts [{string.Join(", ", _unitOfWork.PropertyRepository.ColumnMapForSortBy.Keys)}] values only";
                    return result;
                }

                byte[] Status = form.Status.Split(',')
                                           .Select(selector => byte
                                           .TryParse(selector, out byte x) ? x : (byte)255)
                                           .Where(x => x != 255)
                                           .ToArray();

                int[] Categories = form.CategoryIds.Split(',')
                                                   .Select(Selector => int
                                                   .TryParse(Selector, out int x) ? x : 0)
                                                   .Where(x => x != 0)
                                                   .ToArray();

                PropertySearchOptions options = new()
                {
                    CategoryIds = Categories,
                    StartPrice = form.StartPrice,
                    EndPrice = form.EndPrice,
                    Search = form.Search,
                    SortBy = form.SortBy,
                    SortByDesc = form.SortByDesc,
                    Zipcode = form.ZipCode,
                    Status = Status,
                    CategoryType = form.CategoryType,
                    TotalBedrooms = form.TotalBedrooms,
                    TotalBathrooms = form.TotalBathrooms
                };
                properties = await _unitOfWork.PropertyRepository.FindAllBySearchOptionsAsync(options);

                properties = properties.Where(properties =>
                                                properties.Status == (byte)PropertyStatus.Active).ToList();

                List<PropertyAdditionalInfo>? propertyAdditionalInfos = new();
                foreach (var property in properties)
                {
                    var additionalInfo = await _unitOfWork.PropertyadditionalInfoRepository.FindByPropertyIdAsync(property.PropertyId);
                    if (additionalInfo != null)
                    {
                        propertyAdditionalInfos.Add(additionalInfo);
                    }
                }


                List<PropertyDetailView> propertyViews = properties.ConvertAll(properties => new PropertyDetailView(properties,
                    propertyAdditionalInfos.SingleOrDefault(property => property.PropertyId == properties.PropertyId),
                    _unitOfWork.PropertyRepository.FindThumbnailPicture(properties.PropertyId)?.PropertyImages)).ToList();

                result.Data = new Pager<PropertyDetailView>(form.PageNumber, form.PageSize, propertyViews);
                result.ServiceStatus = ServiceStatus.Success;
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
    }
}
