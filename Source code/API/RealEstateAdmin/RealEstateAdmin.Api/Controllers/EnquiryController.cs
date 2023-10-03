using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;

namespace RealEstateAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/enquiry")]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;
        public EnquiryController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;

        }

        /// <summary>
        /// Get Enquiry Lit
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            if (ModelState.IsValid)
            {
                ServiceResult result = await _enquiryService.ListEnquiry();
                return StatusCode((int)result.ServiceStatus, result);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        /// <summary>
        /// Change Enquiry Status
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("change-status/{enquiryId:int:min(1)}")]
        public async Task<IActionResult> ChangeStatus(int enquiryId, [FromBody] byte status)
        {
            ServiceResult result = await _enquiryService.ChangeStatus(enquiryId, status);
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Get Enwuiry
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{enquiryId:int:min(1)}")]
        public async Task<ActionResult<ServiceResult>> GetEnquiry(int enquiryId)
        {
            if (enquiryId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _enquiryService.GetEnquiryAsync(enquiryId);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Get Enquiry Count
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> Enquiries()
        {
            if (ModelState.IsValid)
            {
                ServiceResult result = await _enquiryService.GetCount();
                return StatusCode((int)result.ServiceStatus, result);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }



    }
}