using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IPropertyUnitService
    {
        Task<ServiceResult> AddPropertyUnit(PropertyUnitForm form, int propertyId);

        Task<ServiceResult> GetUnitbyId(int propertyUnitsId);

        Task<ServiceResult> GetUnitListByProperty(int propertyId);

        Task<ServiceResult> EditPropertyUnit(PropertyUnitForm form, int propertyUnitId);

        Task<ServiceResult> DeletePropertyUnit(int propertyUnitId);
    }
}
