using System.Linq.Expressions;
using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Dictionary<string, Expression<Func<Property, object>>> ColumnMapForSortBy { get; }
        Image? FindThumbnailPicture(int propertyId);
        Task<List<Property>> FindAllByCategoryLikeAndPriceBetweenAsync(int[] categoryIds, string? Search, string? SortBy, bool SortByDesc, byte[] status);
        Task<Property?> FindByIdAsync(int propertyId);
        Image AddImage(Image image);

        Task<Property?> FindByAddressAsync(string address, double latitude, double longitude);
    }
}
