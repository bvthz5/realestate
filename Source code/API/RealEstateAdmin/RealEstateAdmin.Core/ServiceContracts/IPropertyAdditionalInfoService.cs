using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.DTO.Forms;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IPropertyAdditionalInfoService
    {
        public Task<PropertyAdditionalInfo> AddIPropertyAdditionalInfo(PropertyForm form, Property property);
    }
}