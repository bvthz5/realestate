using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private static readonly string _primaryKey = "UserId";
        protected readonly RealEstateDbContext _context;

        public UserRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public Dictionary<string, Expression<Func<User, object>>> ColumnMapForSortBy { get; } = new()
        {
            [_primaryKey] = user => user.UserId,
            ["FirstName"] = user => user.FirstName,
            ["Email"] = user => user.Email,
            ["CreatedDate"] = user => user.CreatedOn
        };

        public async Task<bool> IsProfilePicExists(string fileName)
        {
            return (await _context.Users.SingleOrDefaultAsync(user => user.ProfilePic == fileName)) != null;
        }
    }

}
