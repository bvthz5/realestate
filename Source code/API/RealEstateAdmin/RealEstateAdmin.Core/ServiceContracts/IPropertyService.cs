using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IPropertyService
    {
        Task<ServiceResult> AddProperty(PropertyForm form);
        Task<ServiceResult> EditProperty(int propertyId, PropertyForm propertyForm);

        Task<ServiceResult> ChangeStatusAsync(int propertyId, byte status);
        Task<ServiceResult> PropertyListAsync(ProductPaginationParams form);
        Task<ServiceResult> GetPropertyAsync(int propertyId);
        Task<ServiceResult> Count();
    }
}
