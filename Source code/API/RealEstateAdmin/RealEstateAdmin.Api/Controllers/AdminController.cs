using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;

namespace RealEstateAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Admin Login
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
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
                ServiceResult result = await _adminService.Login(form);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

/// <summary>
/// Refresh Token
/// </summary>
/// <param name="refreshToken"></param>
/// <returns></returns>
        [AllowAnonymous]
        [HttpPut("token-refresh")]
        public async Task<ActionResult<LoginView>> Refresh([FromBody] string refreshToken)
        {
            if (refreshToken == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _adminService.RefreshAsync(refreshToken);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }
    }
}
