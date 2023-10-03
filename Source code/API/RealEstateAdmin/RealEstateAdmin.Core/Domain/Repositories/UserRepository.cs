using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly RealEstateDbContext _context;
        public UserRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<User?> FindByEmail(string email)
        {
            return _context.Users.SingleOrDefaultAsync(user => user.Email == email);
        }

        public Task<User?> FindByUserId(int userId)
        {
            return _context.Users.SingleOrDefaultAsync(user => user.UserId == userId);
        }

        public async Task<User?> FindByIdAsync(int userId)
        {
            return await _context.Users

                                            .SingleOrDefaultAsync(users => users.UserId == userId);
        }

        public async Task<bool> IsProfilePicExists(string fileName)
        {
            return (await _context.Users.SingleOrDefaultAsync(user => user.ProfilePic == fileName)) != null;
        }
    }
}
