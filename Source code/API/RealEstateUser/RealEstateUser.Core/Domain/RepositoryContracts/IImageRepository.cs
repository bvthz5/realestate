using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Task<List<Image>> FindByProductIdAsync(int propertyId);
    }
}
