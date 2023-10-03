using RealEstate.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IEnquiryRepository: IGenericRepository<Enquiry>
    {
        Task<Enquiry?> FindByIdAsync(int enquiryId);
        Task<List<Enquiry>> GetEnquiryList();

    }
}
