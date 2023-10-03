using System.Linq.Expressions;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.Options;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Dictionary<string, Expression<Func<Property, object>>> ColumnMapForSortBy { get; }
        Task<Property?> FindByIdAsync(int propertyId);

        Task<List<Property>> FindAllBySearchOptionsAsync(PropertySearchOptions options);
        Task<List<Property>> FindByUserIAsync(int userId);

        Image? FindThumbnailPicture(int propertyId);
    }
}
