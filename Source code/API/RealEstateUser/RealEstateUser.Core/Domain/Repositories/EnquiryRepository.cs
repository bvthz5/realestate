using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.Extensions;

namespace RealEstateUser.Core.Domain.Repositories
{
    public class EnquiryRepository : GenericRepository<Enquiry>, IEnquiryRepository
    {
        private readonly RealEstateDbContext _context;

        private static readonly string _primaryKey = "EnquiryId";
        public EnquiryRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public Dictionary<string, Expression<Func<Enquiry, object>>> ColumnMapForSortBy { get; } = new()
        {
            [_primaryKey] = enquiry => enquiry.EnquiryId ,
            ["CreatedDate"] = enquiry => enquiry.CreatedOn,
        };

        public async Task<List<Enquiry>> FindAllByCategoryType(string? Search, string? SortBy, bool SortByDesc, byte enquiryType, byte[] status)
        {
            Console.WriteLine(status);

            return await _context.UserEnquries
                                             .Include(property=> property.Property)
                                             .ThenInclude(enquiry => enquiry.Category)
                                             .Include(enquiry=>enquiry.User)
                                             .Where(enquiry => (status.Length == 0 || status.Contains(enquiry.Status))
                                             && (enquiryType == 0 || enquiry.EnquiryType == enquiryType) &&
                                             (string.IsNullOrWhiteSpace(Search) || enquiry.Property.Address.Contains(Search)))
                                             .ApplyOrdering(SortBy ?? _primaryKey, SortByDesc, ColumnMapForSortBy)
                                             .ToListAsync();
        }
        public async Task<Enquiry?> FindByIdAsync(int enquiryId)
        {
            return await _context.UserEnquries.Include(enquiry => enquiry.Property)
                                                    .ThenInclude(property => property.Category)
                                                .SingleOrDefaultAsync(enquiries => enquiries.EnquiryId == enquiryId);
        }

        public async Task<List<Enquiry>> GetEnquiryList()
        {
            return await _context.UserEnquries.Include(enquiry => enquiry.Property)
                                                    .ThenInclude(property => property.Category).ToListAsync();
        }


    }
}

