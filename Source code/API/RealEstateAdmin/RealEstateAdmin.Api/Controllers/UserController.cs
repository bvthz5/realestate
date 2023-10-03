using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using RealEstateAdmin.Core.Services;

namespace RealEstateAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {            
            _userService = userService;
        }

        /// <summary>
        /// Get User Details By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("detail/{userId:int:min(1)}")]
        public async Task<ActionResult<UserDetailView>> GetUser(int userId)
        {

            if (userId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.GetUserAsync(userId);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Get Users List
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sortBy"></param>
        /// <param name="isSortAscending"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> UsersList(string? searchQuery, string? sortBy, bool isSortAscending)
        {
            ServiceResult result = await _userService.GetUserList(searchQuery, sortBy, isSortAscending);
            return StatusCode((int)result.ServiceStatus, result);

        }

        /// <summary>
        /// Change User Status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("change-status/{userId:int:min(1)}")]
        public async Task<IActionResult> ChangeUserStatus(int userId, [FromBody] byte status)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            ServiceResult result = await _userService.ChangeStatusAsync(userId, status);
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Get Profile Picture
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("profile-image/{fileName}")]
        public async Task<IActionResult> GetProfilePic(string fileName)
        {

            FileStream? fileStream = await _userService.GetProfile(fileName);

            if (fileStream is null)
                return NotFound();

            return File(fileStream, "image/jpeg");

        }

        /// <summary>
        /// Get User Count
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> Users()
        {
            if (ModelState.IsValid)
            {
                ServiceResult result = await _userService.Count();
                return StatusCode((int)result.ServiceStatus, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
