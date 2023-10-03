using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.Enums;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class WishListRepository : GenericRepository<WishList>, IWishListRepository
    {
        private readonly RealEstateDbContext _context;
        public WishListRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WishList> AddAsync(WishList wishList)
        {
            return (await _context.AddAsync(wishList)).Entity;
        }

        public async Task<Property?> FindByIdAsync(int propertyId)
        {
            return await _context.Properties
                                            .Include(property => property.Category)
                                            .SingleOrDefaultAsync(property => property.PropertyId == propertyId);
        }

        public async Task<List<WishList>> FindByUserIdAsync(int userId)
        {
            return await _context.WishLists
                                        .Include(wishList => wishList.Property)
                                            .ThenInclude(property => property.Category)
                                        .Include(cart => cart.Property)
                                        .Include(wishList => wishList.Property)
                                        .Include(wishList => wishList.User)
                                        .Where(wishList => wishList.UserId == userId && (wishList.Property.Status== ((byte)PropertyStatus.Active) || wishList.Property.Status == ((byte)PropertyStatus.Occupied)))
                                        .ToListAsync();
        }

        public async Task DeleteByProductIdAndUserIdAsync(int propertyId, int userId)
        {
            WishList? wishList = (await Find(WishLists => WishLists.PropertyId == propertyId && WishLists.UserId == userId)).SingleOrDefault();

            if (wishList != null)
                _context.WishLists.Remove(wishList);
        }
    }
}
