using System.Linq.Expressions;
using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IEnquiryRepository : IGenericRepository<Enquiry>
    {
        Dictionary<string, Expression<Func<Enquiry, object>>> ColumnMapForSortBy { get; }

        Task<List<Enquiry>> FindAllByCategoryType(string? Search, string? SortBy, bool SortByDesc,  byte enquiryType, byte[] status);
        Task<Enquiry?> FindByIdAsync(int enquiryId);
        Task<List<Enquiry>> GetEnquiryList();
    }
}

