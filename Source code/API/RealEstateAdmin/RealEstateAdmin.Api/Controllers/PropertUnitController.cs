using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;

namespace RealEstateAdmin.Api.Controllers
{
    [Route("api/property-unit")]
    [ApiController]
    public class PropertUnitController:ControllerBase
    {
        private readonly IPropertyUnitService _propertyUnitService;
        public PropertUnitController(IPropertyUnitService propertyUnitService)
        {
            _propertyUnitService = propertyUnitService; 
        }

        [HttpPost("add-new/{propertyId}")]
        public async Task<IActionResult> Add(PropertyUnitForm form, int propertyId)
        {
            if (form == null)
            {
                return BadRequest();
            }
            ServiceResult result = await _propertyUnitService.AddPropertyUnit(form, propertyId);
            return StatusCode((int)result.ServiceStatus, result);
        }

        [HttpGet("{propertyUnitsId}")]
        public async Task<IActionResult> GetUnit(int propertyUnitsId)
        {

            ServiceResult result = await _propertyUnitService.GetUnitbyId(propertyUnitsId);
            return StatusCode((int)result.ServiceStatus, result);
        }
        [HttpPut("edit/{propertyUnitsId}")]
        public async Task<IActionResult> UpdateUnit(PropertyUnitForm form, int propertyUnitsId)
        {
            if (form == null)
            {
                return BadRequest();
            }
            ServiceResult result = await _propertyUnitService.EditPropertyUnit(form, propertyUnitsId);
            return StatusCode((int)result.ServiceStatus, result);
        }

        [HttpPut("delete/{propertyUnitsId}")]
        public async Task<IActionResult> DeleteUnit(int propertyUnitsId)
        {
            
            ServiceResult result = await _propertyUnitService.DeletePropertyUnit(propertyUnitsId);
            return StatusCode((int)result.ServiceStatus, result);
        }

        [HttpGet("list/{propertyId}")]
        public async Task<IActionResult> UnitListByPropertyId(int propertyId)
        {

            ServiceResult result = await _propertyUnitService.GetUnitListByProperty(propertyId);
            return StatusCode((int)result.ServiceStatus, result);
        }
    }
}
