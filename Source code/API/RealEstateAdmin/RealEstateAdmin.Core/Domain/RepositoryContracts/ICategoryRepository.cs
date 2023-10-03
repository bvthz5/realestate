using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> FindByCategoryNameAsync(string categoryName);
        Task<Category?> FindByIdAndStatusAsync(int categoryId, byte status);
    }
}
