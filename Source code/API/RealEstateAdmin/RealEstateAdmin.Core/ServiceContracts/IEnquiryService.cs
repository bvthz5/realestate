using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IEnquiryService
    {
        Task<ServiceResult> ListEnquiry();
        Task<ServiceResult> GetEnquiryAsync(int enquiryId);

        Task<ServiceResult> GetCount();
        Task<ServiceResult> ChangeStatus(int enquiryId, byte status);
    }
}
