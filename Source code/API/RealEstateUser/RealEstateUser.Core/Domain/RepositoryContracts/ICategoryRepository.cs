using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> FindAllByStatusAsync(byte status);
    }
}
