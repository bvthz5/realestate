using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;

namespace RealEstateAdmin.Core.Services
{
    public class PropertyUnitService : IPropertyUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PropertyService> _logger;

        public PropertyUnitService(IUnitOfWork unitOfWork, ILogger<PropertyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<ServiceResult> AddPropertyUnit(PropertyUnitForm form, int propertyId)
        {
            ServiceResult result = new();

            Property property = await _unitOfWork.PropertyRepository.GetById(propertyId);

            if (property == null)
            {
                result.Message = "Property not found";
                result.ServiceStatus = ServiceStatus.NoRecordFound;
                return result;
            }

            var unitList = await _unitOfWork.PropertyUnitRepository.FindActiveListByPropertyId(propertyId);

            if (unitList == null || unitList.Count > 50)
            {
                result.Message = "Limin Exceeded. Only 50 Units allowed";
                result.ServiceStatus = ServiceStatus.NoRecordFound;
                return result;
            }

            PropertyUnits unit = new()
            {
                AvailabilityStatus = form.AvailabilityStatus,
                AvailableFrom = form.AvailableFrom,
                CreatedBy = 1,
                IsActive = true,
                IsDeleted = false,
                PropertyId = propertyId,
                UnitFeatures = form.UnitFeatures,
                UnitName = form.UnitName,
                UpdatedBy = 1,
                Price = form.Price,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                TotalBedrooms = form.TotalBedrooms,
                TotalBathrooms = form.TotalBathrooms,
                SquareFootage = form.SquareFootage,
                SecurityDeposit = form.SecurityDeposit
            };

            unit = await _unitOfWork.PropertyUnitRepository.Add(unit);
            await _unitOfWork.SaveAsync();

            result.Data = unit;
            result.Message = "unit added";
            return result;
        }

        public async Task<ServiceResult> EditPropertyUnit(PropertyUnitForm form, int propertyUnitId)
        {
            ServiceResult result = new();

            PropertyUnits unit = await _unitOfWork.PropertyUnitRepository.GetById(propertyUnitId);

            if (unit == null)
            {
                result.Message = "Unit not found";
                result.ServiceStatus = ServiceStatus.NoRecordFound;
                return result;
            }


            unit.AvailabilityStatus = form.AvailabilityStatus;
            unit.AvailableFrom = form.AvailableFrom;
            unit.UnitFeatures = form.UnitFeatures;
            unit.UnitName = form.UnitName;
            unit.UpdatedBy = 1;
            unit.Price = form.Price;
            unit.UpdatedOn = DateTime.Now;
            unit.TotalBedrooms = form.TotalBedrooms;
            unit.TotalBathrooms = form.TotalBathrooms;
            unit.SquareFootage = form.SquareFootage;
            unit.SecurityDeposit = form.SecurityDeposit;


            unit = _unitOfWork.PropertyUnitRepository.Update(unit);
            await _unitOfWork.SaveAsync();

            result.Data = unit;
            result.Message = "unit added";
            return result;
        }

        public async Task<ServiceResult> DeletePropertyUnit(int propertyUnitId)
        {
            ServiceResult result = new();

            PropertyUnits unit = await _unitOfWork.PropertyUnitRepository.GetById(propertyUnitId);

            if (unit == null)
            {
                result.Message = "Unit not found";
                result.ServiceStatus = ServiceStatus.NoRecordFound;
                return result;
            }

            unit.IsDeleted = true;
            unit.IsActive = false;

            _unitOfWork.PropertyUnitRepository.Update(unit);
            await _unitOfWork.SaveAsync();

            result.Message = "Unit deleted";
            return result;
        }

        public async Task<ServiceResult> GetUnitbyId(int propertyUnitsId)
        {
            ServiceResult result = new();

            PropertyUnits? unit = await _unitOfWork.PropertyUnitRepository.FindByIdAsync(propertyUnitsId);
          
            if (unit == null)
            {
                result.Message = "Unit not found";
                result.ServiceStatus = ServiceStatus.NoRecordFound;
            }
            result.Data = unit;
            return result;
        }

        public async Task<ServiceResult> GetUnitListByProperty(int propertyId)
        {
            ServiceResult result = new();

            List<PropertyUnits>? unitList = await _unitOfWork.PropertyUnitRepository.FindActiveListByPropertyId(propertyId);

            if (unitList == null || unitList.Count == 0)
            {
                result.Message = "No units found";
                result.ServiceStatus = ServiceStatus.NoRecordFound;
            }
            result.Data = unitList;
            return result;
        }
    }
}
