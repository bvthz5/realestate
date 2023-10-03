using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;

namespace RealEstateAdmin.Api.Controllers
{
    [Route("api/property")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        /// <summary>
        /// Add Property
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add-new")]
        public async Task<IActionResult> Add(PropertyForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            ServiceResult result = await _propertyService.AddProperty(form);
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Get Property List
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> PaginatedPropertyList([FromQuery] ProductPaginationParams form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _propertyService.PropertyListAsync(form);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Edit Property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyForm"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("edit/{propertyId:int:min(1)}")]
        public async Task<IActionResult> EditProperty(int propertyId, PropertyForm propertyForm)
        {
            ServiceResult result = await _propertyService.EditProperty(propertyId, propertyForm);
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Change Property Status
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("change-status/{propertyId:int:min(1)}")]
        public async Task<IActionResult> ChangePropertyStatus(int propertyId, [FromBody] byte status)
        {
            ServiceResult result = await _propertyService.ChangeStatusAsync(propertyId, status);
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Get Property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("detail/{propertyId:int:min(1)}")]
        public async Task<IActionResult> GetProperty(int propertyId)
        {
            if (propertyId == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok(await _propertyService.GetPropertyAsync(propertyId));
            }
        }

        /// <summary>
        /// Get Property Count
        /// </summary>
        /// <returns></returns>

        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> Property()
        {
            if (ModelState.IsValid)
            {
                ServiceResult result = await _propertyService.Count();
                return StatusCode((int)result.ServiceStatus, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}