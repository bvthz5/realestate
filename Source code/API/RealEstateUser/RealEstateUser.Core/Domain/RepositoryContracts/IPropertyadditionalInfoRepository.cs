using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IPropertyadditionalInfoRepository : IGenericRepository<PropertyAdditionalInfo>
    {
        Task<PropertyAdditionalInfo?> FindByPropertyIdAsync(int propertyId);

    }
}
