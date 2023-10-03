using System.Linq.Expressions;
using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Dictionary<string, Expression<Func<User, object>>> ColumnMapForSortBy { get; }

        Task<bool> IsProfilePicExists(string fileName);
    }
}
