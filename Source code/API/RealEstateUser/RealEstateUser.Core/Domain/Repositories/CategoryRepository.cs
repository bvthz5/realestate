using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly RealEstateDbContext _context;
        public CategoryRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> FindAllByStatusAsync(byte status)
        {
            return await _context.Categories
                                        .Where(category => category.Status == status)
                                        .OrderBy(category => category.CategoryName)
                                        .ToListAsync();
        }
    }
}
