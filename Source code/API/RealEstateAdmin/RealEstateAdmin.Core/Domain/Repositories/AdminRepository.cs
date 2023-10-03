
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class AdminRepository : GenericRepository<User>, IAdminRepository
    {
        private readonly RealEstateDbContext _context;
        public AdminRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<Admin?> FindByEmail(string email)
        {
            return _context.Admins.SingleOrDefaultAsync(admin => admin.Email == email);
        }

        public Task<Admin?> FindByAdminId(int adminId)
        {
            return _context.Admins.SingleOrDefaultAsync(admin => admin.AdminId == adminId);
        }

        public async Task<User?> FindByIdAsync(int userId)
        {
            return await _context.Users

                                            .SingleOrDefaultAsync(users => users.UserId == userId);
        }
    }
}
