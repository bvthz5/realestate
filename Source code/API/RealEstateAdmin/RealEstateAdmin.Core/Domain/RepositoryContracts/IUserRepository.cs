using RealEstate.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> FindByEmail(string email);
        Task<User?> FindByUserId(int userId);
        Task<User?> FindByIdAsync(int userId);
        Task<bool> IsProfilePicExists(string fileName);
    }
}
