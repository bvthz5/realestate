using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.Options;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Extensions;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly RealEstateDbContext _context;
        private static readonly string _primaryKey = "PropertyId";
        public PropertyRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public Dictionary<string, Expression<Func<Property, object>>> ColumnMapForSortBy { get; } = new()
        {
            [_primaryKey] = Properties => Properties.PropertyId,
            ["Price"] = Properties => Properties.Price,
            ["CreatedDate"] = Properties => Properties.CreatedOn,
            ["SquareFootage"] = Properties => Properties.SquareFootage,
            ["TotalBedrooms"] = properties => properties.TotalBedrooms,
            ["TotalBathrooms"] = proprties => proprties.TotalBathrooms,
            ["Rent"] = Property => ((CategoryType)Property.Category.Type).ToString(),
            ["Buy"] = Property => ((CategoryType)Property.Category.Type).ToString(),
        };

        public async Task<Property?> FindByIdAsync(int propertyId)
        {
            return await _context.Properties.Include(Properties => Properties.Category)
                                            .SingleOrDefaultAsync(Properties => Properties.PropertyId == propertyId);
        }

        public async Task<List<Property>> FindAllBySearchOptionsAsync(PropertySearchOptions options)
        {
            return await _context.Properties
                .Include(p => p.Category)
                .Where(p => (options.CategoryIds.Length == 0 || options.CategoryIds.Contains(p.CategoryId)) &&
                            (options.Status.Length == 0 || options.Status.Contains(p.Status)) &&
                            p.Price >= options.StartPrice &&
                            (options.EndPrice == 0.0 || p.Price <= options.EndPrice) &&
                            (options.CategoryType == 0 || p.Category.Type == options.CategoryType) &&
                            p.TotalBathrooms >= options.TotalBathrooms &&
                            p.TotalBedrooms >= options.TotalBedrooms &&
                            (string.IsNullOrWhiteSpace(options.Zipcode) || p.ZipCode == options.Zipcode) &&
                            (string.IsNullOrWhiteSpace(options.Search) || p.Address.Contains(options.Search) || p.City.Contains(options.Search) || p.ZipCode.Contains(options.Search)))
                .ApplyOrdering(options.SortBy ?? _primaryKey, options.SortByDesc, ColumnMapForSortBy)
                .ToListAsync();
        }


        public async Task<List<Property>> FindByUserIAsync(int userId)
        {
            return await _context.Properties
                                        .Include(Properties => Properties.CreatedBy)
                                        .Include(Properties => Properties.Category)
                                         .Where(product => product.CreatedBy == userId)
                                        .ToListAsync();
        }

        public Image? FindThumbnailPicture(int propertyId)
        {
            return _context.Images.FirstOrDefault(image => image.PropertyId == propertyId);
        }
    }
}
