using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class PropertyUnitRepository : GenericRepository<PropertyUnits>, IPropertyUnitRepository
    {

        private readonly RealEstateDbContext _context;
        public PropertyUnitRepository(RealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PropertyUnits?> FindByIdAsync(int propertyUnitsId)
        {
            
                return await _context.PropertyUnits
                                                .Include(unit => unit.Property)
                                                .SingleOrDefaultAsync(unit => unit.PropertyUnitsId == propertyUnitsId);
            
        }

        public async Task<List<PropertyUnits>?> FindActiveList()
        {
            return await _context.PropertyUnits.Where(unit => unit.IsActive).ToListAsync();
        }

        public async Task<List<PropertyUnits>?> FindActiveListByPropertyId(int propertyId)
        {
            return await _context.PropertyUnits.Where(unit => unit.IsActive && unit.PropertyId == propertyId).ToListAsync();
        }
    }
}
