using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class EnquiryRepository : GenericRepository<Enquiry>, IEnquiryRepository
    {
        private readonly RealEstateDbContext _context;
        public EnquiryRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
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
