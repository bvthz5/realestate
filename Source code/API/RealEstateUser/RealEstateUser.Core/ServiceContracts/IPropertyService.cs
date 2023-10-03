using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface IPropertyService
    {
        Task<ServiceResult> GetPropertyAsync(int propertyId);
        Task<ServiceResult> PropertyListAsync(PropertyPaginationForm form);

    }
}
