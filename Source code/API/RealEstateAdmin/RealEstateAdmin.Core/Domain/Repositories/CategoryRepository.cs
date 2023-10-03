using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly RealEstateDbContext _context;
        public CategoryRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Category?> FindByCategoryNameAsync(string categoryName)
        {
            return await _context.Categories.SingleOrDefaultAsync(category => category.CategoryName.ToLower() == categoryName);
        }
        public async Task<Category?> FindByIdAndStatusAsync(int categoryId, byte status)
        {
            return await _context.Categories.SingleOrDefaultAsync(category => category.CategoryId == categoryId && category.Status == status);
        }
    }
}
