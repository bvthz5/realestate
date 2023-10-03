using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.DTO.Views;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Core.Services
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WishListService> _logger;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public WishListService(IUnitOfWork unitOfWork, ILogger<WishListService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResult> AddToWishList(int userId, int propertyId)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }
                var property = await _unitOfWork.WishListRepository.FindByIdAsync(propertyId);

                if (property == null || property.Status != (byte)PropertyStatus.Active)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"Property Not Found";

                    if (property != null)
                    {
                        result.Message += $" : {(PropertyStatus)property.Status}";
                    }
                    return result;
                }
                var w = await _unitOfWork.WishListRepository.Find(WishLists => WishLists.PropertyId == propertyId && WishLists.UserId == userId);


                if (w.Any())
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Property Already Added";

                    return result;
                }

                var wishList = new WishList()
                {
                    UserId = userId,
                    PropertyId = propertyId
                };

                await _unitOfWork.WishListRepository.AddAsync(wishList);

                await _unitOfWork.SaveAsync();

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = "Property Added To WishList";

                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = $"WS-01:ServerError";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> GetWishList(int userId)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }

                var wishList = await _unitOfWork.WishListRepository.FindByUserIdAsync(userId);

                if (!wishList.Any())
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Wishlist Empty for the user";
                    return result;
                }

                List<PropertyAdditionalInfo>? propertyAdditionalInfos = new();
                foreach (var property in wishList)
                {
                    var additionalInfo = await _unitOfWork.PropertyadditionalInfoRepository.FindByPropertyIdAsync(property.PropertyId);
                    if (additionalInfo != null)
                    {
                        propertyAdditionalInfos.Add(additionalInfo);
                    }
                }

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = wishList.ConvertAll(w => new WishListView(w.Property, propertyAdditionalInfos.SingleOrDefault(property => property.PropertyId == w.PropertyId), _unitOfWork.PropertyRepository.FindThumbnailPicture(w.PropertyId)?.PropertyImages));
                return result;

            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = $"WS-02:ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;

        }

        public async Task<ServiceResult> RemoveFromWishList(int userId, int propertyId)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }

                if ((await _unitOfWork.WishListRepository.Find(WishLists => WishLists.PropertyId == propertyId && WishLists.UserId == userId)).SingleOrDefault() == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";

                    return result;
                }
                await _unitOfWork.WishListRepository.DeleteByProductIdAndUserIdAsync(propertyId, userId);

                await _unitOfWork.SaveAsync();

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = "Removed From WishList";

                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = $"WS-03:ServerError";  
                _logger.LogError(e, ErrorMessageTemplate, e.Message);

            }
            return result;
        }
    }

}
