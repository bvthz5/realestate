using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.Domain.RepositoryContracts
{
    public interface IPropertyUnitRepository : IGenericRepository<PropertyUnits>
    {
        Task<PropertyUnits?> FindByIdAsync(int propertyUnitsId);

        Task<List<PropertyUnits>?> FindActiveList();

        Task<List<PropertyUnits>?> FindActiveListByPropertyId(int propertyId);

    }
}
