using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
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
        /// Get a property by ID
        /// </summary>
        /// <param name="propertyId">The ID of the property to get</param>
        /// <returns>The result of the service operation</returns>

        [AllowAnonymous]
        [HttpGet("{propertyId:int:min(1)}")]
        public async Task<IActionResult> Getproperty(int propertyId)
        {

            if (propertyId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _propertyService.GetPropertyAsync(propertyId);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Get a paginated list of property
        /// </summary>
        /// <param name="form">The pagination parameters</param>
        /// <returns>The result of the service operation</returns>

        [AllowAnonymous]
        [HttpGet("page")]
        public async Task<IActionResult> PaginatedPropertyList([FromQuery] PropertyPaginationForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _propertyService.PropertyListAsync(form);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }

        }
    }
}

