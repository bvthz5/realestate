using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IWishListRepository : IGenericRepository<WishList>
    {
        Task<WishList> AddAsync(WishList wishList);
        Task<Property?> FindByIdAsync(int propertyId);
        Task<List<WishList>> FindByUserIdAsync(int userId);
        Task DeleteByProductIdAndUserIdAsync(int propertyId, int userId);
    }
}
