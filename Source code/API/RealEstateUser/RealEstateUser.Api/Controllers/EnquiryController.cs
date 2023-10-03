using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/enquiry")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;
        private readonly SecurityUtil _securityUtil;
        public EnquiryController(IEnquiryService enquiryService, SecurityUtil securityUtil)
        {
            _enquiryService = enquiryService;
            _securityUtil = securityUtil;
        }

        /// <summary>
        /// request a new enquiry
        /// </summary>
        /// <param name="form">The form data for the new Enquiry</param>
        /// <param>The _securityUtil.GetCurrentUserId() method is used to retrieve the current user's ID</param>
        /// <returns>The result of the service operation</returns>

        [Authorize]
        [HttpPost("request-tour")]
        public async Task<IActionResult> Tour(EnquiryTourForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _enquiryService.RequestTour(form, _securityUtil.GetCurrentUserId());

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Get a paginated list of equiry
        /// </summary>
        /// <param name="form">The pagination parameters</param>
        /// <returns>The result of the service operation</returns>

        [Authorize]
        [HttpGet("page")]
        public async Task<IActionResult> PaginatedEnquiryList([FromQuery] EnquiryPaginationForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            ServiceResult result = await _enquiryService.EnquiryList(form, _securityUtil.GetCurrentUserId());

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Get a enquiry by ID
        /// </summary>
        /// <param name="enquiryId">The ID of the enquiry to get</param>
        /// <returns>The result of the service operation</returns>

        [AllowAnonymous]
        [HttpGet("{enquiryId:int:min(1)}")]
        public async Task<IActionResult> GetEnquiry(int enquiryId)
        {
            if (enquiryId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _enquiryService.GetEnquiryAsync(enquiryId);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }
    }
}


