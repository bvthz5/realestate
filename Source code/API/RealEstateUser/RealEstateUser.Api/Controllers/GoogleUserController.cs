using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/google")]
    [ApiController]
    public class GoogleUserController : ControllerBase
    {
        private readonly IGoogleService _googleService;

        public GoogleUserController(IGoogleService googleService)
        {
            _googleService = googleService;
        }

        /// <summary>
        /// Controller method for logging in with a Google account.
        /// </summary>

        [AllowAnonymous]
        [HttpPost("user-login")]
        public async Task<IActionResult> Login([FromBody] string idToken)
        {
            // Call the service to register and login with the Google account.
            ServiceResult result = await _googleService.Login(idToken);

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }
    }
}
