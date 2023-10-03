using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface IEnquiryService
    {
        Task<ServiceResult> RequestTour(EnquiryTourForm form,  int userId);
        Task<ServiceResult> EnquiryList(EnquiryPaginationForm form, int userId);
        Task<ServiceResult> GetEnquiryAsync(int enquiryId);

    }
}
