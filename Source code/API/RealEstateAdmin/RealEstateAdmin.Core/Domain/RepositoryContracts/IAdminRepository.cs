using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IAdminRepository : IGenericRepository<User>
    {
        Task<Admin?> FindByEmail(string email);
        Task<Admin?> FindByAdminId(int adminId);
        Task<User?> FindByIdAsync(int userId);
    }
}
