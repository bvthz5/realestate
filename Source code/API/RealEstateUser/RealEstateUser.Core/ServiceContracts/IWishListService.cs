using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface IWishListService
    {
        Task<ServiceResult> AddToWishList(int userId, int propertyId);

        Task<ServiceResult> RemoveFromWishList(int userId, int propertyId);

        Task<ServiceResult> GetWishList(int userId);

    }
}
