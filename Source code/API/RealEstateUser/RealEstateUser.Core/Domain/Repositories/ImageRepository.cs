using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly RealEstateDbContext _context;

        public ImageRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Image>> FindByProductIdAsync(int propertyId)
        {
            return await _context.Images.Where(image => image.PropertyId == propertyId).ToListAsync();
        }

    }
}
