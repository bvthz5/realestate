using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class PropertyAdditionalInfoRepository : GenericRepository<PropertyAdditionalInfo>, IPropertyAdditionalInfoRepository
    {
        

        public PropertyAdditionalInfoRepository(RealEstateDbContext context) : base(context)
        {
            
        }
    }
}
