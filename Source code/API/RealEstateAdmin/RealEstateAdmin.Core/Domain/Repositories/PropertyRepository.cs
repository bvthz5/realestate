using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Extensions;
using System.Linq.Expressions;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly RealEstateDbContext _context;
        private static readonly string _primaryKey = "PropertyId";
        public Dictionary<string, Expression<Func<Property, object>>> ColumnMapForSortBy { get; } = new()
        {
            [_primaryKey] = property => property.PropertyId,
            ["Price"] = property => property.Price,
            ["CreatedDate"] = property => property.CreatedOn,
            ["TotalBedrooms"] = property => property.TotalBedrooms,
            ["TotalBathrooms"] = Property => Property.TotalBathrooms,
            ["SquareFootage"] = Property => Property.SquareFootage,
            ["Rent"] = Property => CategoryType.Rent,
            ["Buy"] = Property => CategoryType.Buy
        };
        public PropertyRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;

        }
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public Property Update(Property property)

        {
            _context.Properties.Update(property);
            return property;
        }
        public Image? FindThumbnailPicture(int propertyId)
        {
            return _context.Images.FirstOrDefault(photo => photo.PropertyId == propertyId);
        }
        public async Task<List<Property>> FindAllByCategoryLikeAndPriceBetweenAsync(int[] categoryIds, string? Search, string? SortBy, bool SortByDesc, byte[] status)
        {

            Console.WriteLine(status);

            return await _context.Properties
                                         .Include(properties => properties.Category)
                                         .Where(properties => (categoryIds.Length == 0 || categoryIds.Contains(properties.CategoryId)) &&
                                                            (status.Length == 0 || status.Contains(properties.Status)) &&
                                                            (properties.Status != (byte)PropertyStatus.Deleted) &&
                                                            (string.IsNullOrWhiteSpace(Search) || properties.Address.Contains(Search) ||
                                                            properties.City.Contains(Search) || properties.ZipCode.Contains(Search)))
                                         .ApplyOrdering(SortBy ?? _primaryKey, SortByDesc, ColumnMapForSortBy)
                                         .ToListAsync();
        }
        public async Task<Property?> FindByIdAsync(int propertyId)
        {
            return await _context.Properties
                                            .Include(properties => properties.Category)
                                            .SingleOrDefaultAsync(properties => properties.PropertyId == propertyId);
        }
        public Image AddImage(Image image)
        {
            return _context.Images.Add(image).Entity;
        }

        public async Task<Property?> FindByAddressAsync(string address, double latitude, double longitude)
        {
            return await _context.Properties.Where(property=>property.Address==address || 
            (property.Latitude == latitude && property.Longitude == longitude)).FirstOrDefaultAsync();
        }
    }
}
