using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;

namespace RealEstateAdmin.Core.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PropertyService> _logger;
        private readonly IPropertyAdditionalInfoService _propertyAdditionalInfoService;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage.}";
        public PropertyService(IUnitOfWork unitOfWork, ILogger<PropertyService> logger, IPropertyAdditionalInfoService propertyAdditionalInfoService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _propertyAdditionalInfoService = propertyAdditionalInfoService;
        }

        /// <summary>
        /// Add New Property
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<ServiceResult> AddProperty(PropertyForm form)
        {
            ServiceResult result = new();
            try
            {

                var properties = (await _unitOfWork.PropertyRepository.Find(property => property.Address == form.Address.Trim()
                || (property.Latitude == form.Latitude && property.Longitude == form.Longitude))).SingleOrDefault();
                if (properties != null)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Property Already Exists";
                    return result;
                }
                if (await _unitOfWork.CategoryRepository.FindByIdAndStatusAsync(form.CategoryId, (byte)CategoryStatus.Active) == null)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Category Id";
                    return result;
                }
                Property property = await _unitOfWork.PropertyRepository.Add(new()
                {
                    Address = form.Address.Trim(),
                    Description = form.Description.Trim(),
                    City = form.City,
                    ZipCode = form.ZipCode,
                    Price = form.Price,
                    CategoryId = form.CategoryId,
                    Latitude = form.Latitude,
                    Longitude = form.Longitude,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Status = (byte)PropertyStatus.Active,
                    TotalBedrooms = form.TotalBedrooms,
                    TotalBathrooms = form.TotalBathrooms,
                    SquareFootage = form.SquareFootage,
                    SecurityDeposit = form.SecurityDeposit,
                });

                await _unitOfWork.SaveAsync();
                await _propertyAdditionalInfoService.AddIPropertyAdditionalInfo(form, property);
                result.Data = new PropertyDetailView(property);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Property Added Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS-01 : Server Error.";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Edit Property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="editPtopertyForm"></param>
        /// <returns></returns>
        public async Task<ServiceResult> EditProperty(int propertyId, PropertyForm propertyForm)
        {
            ServiceResult result = new();
            try
            {
                Property? property = (await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId));
                if (property is null || property.Status == (byte)PropertyStatus.Deleted)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";
                    return result;
                }

                Property? propert = await _unitOfWork.PropertyRepository.FindByAddressAsync(propertyForm.Address, propertyForm.Latitude, propertyForm.Longitude);


                if (propert != null && propert.PropertyId != propertyId)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Address Already Exists";
                    return result;
                }

                Category? category = (await _unitOfWork.CategoryRepository.Find(category => category.CategoryId == propertyForm.CategoryId)).SingleOrDefault();
                if (category is null)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid CategoryId";
                    return result;
                }
                property.Address = propertyForm.Address.Trim();
                property.Description = propertyForm.Description.Trim();
                property.City = propertyForm.City;
                property.ZipCode = propertyForm.ZipCode;
                property.Price = (float)propertyForm.Price;
                property.CategoryId = propertyForm.CategoryId;
                property.Category = category;
                property.Latitude = propertyForm.Latitude;
                property.Longitude = propertyForm.Longitude;
                property.UpdatedOn = DateTime.Now;
                property.TotalBedrooms = propertyForm.TotalBedrooms;
                property.TotalBathrooms = propertyForm.TotalBathrooms;
                property.SquareFootage = propertyForm.SquareFootage;
                property.SecurityDeposit = propertyForm.SecurityDeposit;
                await _unitOfWork.SaveAsync();
                await _propertyAdditionalInfoService.AddIPropertyAdditionalInfo(propertyForm, property);
                result.Data = new PropertyDetailView(property);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Details Edited Successfully";
                return result;

            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS-02 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// List Property
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<ServiceResult> PropertyListAsync(ProductPaginationParams form)
        {
            ServiceResult result = new();
            try
            {
                List<Property>? properties;
                if (form.SortBy != null && !_unitOfWork.PropertyRepository.ColumnMapForSortBy.ContainsKey(form.SortBy))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = $"SortBy : Accepts [{string.Join(", ", _unitOfWork.PropertyRepository.ColumnMapForSortBy.Keys)}] values only";
                    return result;
                }

                byte[] status = form.Status.Split(',')
                                  .Select(s => byte.TryParse(s, out byte n) ? n : (byte)255)
                                  .Where(x => x != 255)
                                  .ToArray();
                int[] categories = form.CategoryIds.Split(',')
                                   .Select(s => int.TryParse(s, out int n) ? n : 0)
                                   .Where(n => n != 0)
                                   .ToArray();
                properties = await _unitOfWork.PropertyRepository.FindAllByCategoryLikeAndPriceBetweenAsync(
                                              categories,
                                              form.Search,
                                              form.SortBy,
                                              form.SortByDesc,
                                              status);

                List<PropertyView> propertyViews = properties.ConvertAll(property => new PropertyView(property, _unitOfWork.PropertyRepository.FindThumbnailPicture(property.PropertyId)?.PropertyImages)).ToList();
                Pager<PropertyView> pager = new(form.PageNumber, form.PageSize, propertyViews);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                result.Data = pager;
                if (properties.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No Property Found";
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS-03 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Change Property Status
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ChangeStatusAsync(int propertyId, byte status)
        {
            ServiceResult result = new();
            try
            {
                Property? property = await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId);

                if (property == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";
                    return result;
                }

                if (status != (byte)PropertyStatus.Active && status != (byte)PropertyStatus.Inactive && status != (byte)PropertyStatus.Occupied && status != (byte)PropertyStatus.SoldOut && status != (byte)PropertyStatus.Deleted)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Status";
                    return result;
                }
                property.Status = status;
                property.UpdatedOn = DateTime.Now;
                _unitOfWork.PropertyRepository.Update(property);
                await _unitOfWork.SaveAsync();
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                result.Data = new PropertyDetailView(property);
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS-04 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get Property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> GetPropertyAsync(int propertyId)
        {
            ServiceResult result = new();
            try
            {

                Property? property = (await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId));
                if (property is null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";
                    return result;
                }
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                result.Data = new PropertyAdditionalInfoView((await _unitOfWork.PropertyAdditionalInfoRepository
                    .Find(propertyAdditionalInfo => propertyAdditionalInfo.PropertyId == property.PropertyId))
                    .SingleOrDefault(), property);
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS-05 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get Property Count
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult> Count()
        {
            ServiceResult result = new();
            try
            {
                List<Property> properties = (await _unitOfWork.PropertyRepository.Find(properties => properties.Status != (byte)PropertyStatus.Deleted)).ToList();

                if (properties.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No Property Found";
                    return result;
                }
                int totalProperties = properties.Count;
                int inactiveProperty = properties.Count(property => property.Status == (byte)PropertyStatus.Inactive);
                int activeProperty = properties.Count(property => property.Status == (byte)PropertyStatus.Active);
                int occupiedProperty = properties.Count(property => property.Status == (byte)PropertyStatus.Occupied);
                int soldoutProperty = properties.Count(property => property.Status == (byte)PropertyStatus.SoldOut);
                var userCounts = new Dictionary<string, int>()
                {
                    { "inactiveProperty", inactiveProperty },
                    { "activeProperty", activeProperty },
                    { "occupiedProperty", occupiedProperty},
                    { "soldoutProperty", soldoutProperty },
                    { "totalProperties", totalProperties }
                };

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = userCounts;
                result.Message = "Property Counts";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "PS-06 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }
    }
}
