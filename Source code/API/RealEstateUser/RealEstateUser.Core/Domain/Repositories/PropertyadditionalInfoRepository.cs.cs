using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class PropertyadditionalInfoRepository : GenericRepository<PropertyAdditionalInfo>, IPropertyadditionalInfoRepository
    {
        private readonly RealEstateDbContext _context;

        public PropertyadditionalInfoRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<PropertyAdditionalInfo?> FindByPropertyIdAsync(int propertyId)
        {
            return await (_context.PropertyInfos.SingleOrDefaultAsync(property => property.PropertyId == propertyId));
        }
    }
}
