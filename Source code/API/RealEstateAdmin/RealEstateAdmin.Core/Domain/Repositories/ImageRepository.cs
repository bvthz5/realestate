using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly RealEstateDbContext _context;
        public ImageRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Image>> FindByPropertyIdAsync(int propertyId)
        {
            return await _context.Images.Where(image => image.PropertyId == propertyId).ToListAsync();
        }
        public void DeleteImages(List<Image> images)
        {
            _context.Images.RemoveRange(images);
        }
        public void DeleteImage(Image image)
        {
            _context.Images.Remove(image);
        }
        public async Task<Image?> FindById(int ImageId)
        {
            return await _context.Images.Include(images => images.Property).FirstOrDefaultAsync(image => image.ImageId == ImageId);
        }
    }
}
