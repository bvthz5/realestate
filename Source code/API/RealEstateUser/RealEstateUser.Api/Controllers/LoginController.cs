using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Controller method for logging in with user credentials.
        /// </summary>
        /// <param name="form">LoginForm containing user credentials.</param>

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                // Call the service to log in with user credentials.
                ServiceResult result = await _userService.Login(form);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Controller method for refreshing an authentication token.
        /// </summary>
        /// <param name="refreshToken">The refresh token used to obtain a new access token.</param>

        [AllowAnonymous]
        [HttpPut("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            if (refreshToken == null)
            {
                return BadRequest();
            }
            else
            {
                // Call the service to refresh the authentication token.
                ServiceResult result = await _userService.Refresh(refreshToken);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }
    }
}
