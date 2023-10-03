using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.DTO.Validations;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SecurityUtil _securityUtil;
        public UserController(IUserService userService, SecurityUtil securityUtil)
        {
            _userService = userService;
            _securityUtil = securityUtil;
        }

        /// <summary>
        /// Adds a new user account to the system
        /// </summary>
        /// <param name="form">User registration form data</param>
        /// <returns>ServiceResult object containing the operation status and any relevant data</returns> 

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.AddUser(form);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        [HttpPut("resend-email")]
        public async Task<IActionResult> ResendVerificationMail([FromBody][Email] string email)
        {
            if(email == null)
            {
                return BadRequest();
            }
            ServiceResult result = await _userService.ResendVerificationMail(email);
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Verifies a user account using a verification token
        /// </summary>
        /// <param name="token">Verification token to use for account verification</param>
        /// <returns>ServiceResult object containing the operation status and any relevant data</returns>

        [HttpPut("verify-email")]
        public async Task<IActionResult> EmailVerification([FromBody] string token)
        {
            if (token == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.VerifyUser(token);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Action method for initiating the forgot password process.
        /// Does not require an authenticated user.
        /// </summary>
        /// <param name="email">The email address associated with the user's account.</param>
        /// <returns>The status of the forgot password request.</returns>

        [AllowAnonymous]
        [HttpPut("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody][Email] string email)
        {
            if (email == null || email.Length == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.ForgotPasswordRequest(email);

                if (result.ServiceStatus == ServiceStatus.Success)
                {
                    // Return the appropriate HTTP status code and result.
                    return StatusCode((int)HttpStatusCode.OK, result);
                }
                else
                {
                    // Return the appropriate HTTP status code and result.
                    return StatusCode((int)HttpStatusCode.BadRequest, result);
                }
            }
        }


        /// <summary>
        /// Action method for resetting the user's password.
        /// Does not require an authenticated user.
        /// </summary>
        /// <param name="form">The form containing the password reset information.</param>
        /// <returns>The status of the password reset request.</returns>

        [AllowAnonymous]
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.ResetPassword(form);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Action method for setting the user's profile picture.
        /// Requires an authenticated user.
        /// </summary>
        /// <param name="image">The form data containing the image to upload.</param>
        /// <returns>The status of the upload operation.</returns>

        [Authorize]
        [HttpPut("profile-pic")]
        public async Task<IActionResult> SetProfilePic([FromForm] ImageForm image)
        {
            if (image == null)
            {
                return BadRequest();
            }

            if (_userService == null || _securityUtil == null)
            {
                // return appropriate error response
                return StatusCode(500, "Internal Server Error");
            }

            ServiceResult result = await _userService.UploadImage(_securityUtil.GetCurrentUserId(), image);

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }


        /// <summary>
        /// Action method for getting the user's profile picture.
        /// Does not require an authenticated user.
        /// </summary>
        /// <param name="fileName">The name of the file to retrieve.</param>
        /// <returns>The user's profile picture file.</returns>

        [HttpGet("profile/{fileName}")]
        public async Task<IActionResult> GetProfilePic(string fileName)
        {
            FileStream? fileStream = await _userService.GetProfile(fileName);

            if (fileStream is null)
                return NotFound();

            return File(fileStream, "image/jpeg");
        }

        /// <summary>
        /// Action method for changing the user's password.
        /// Requires an authenticated user.
        /// </summary>
        /// <param name="form">The form data containing the password change information.</param>
        /// <returns>The status of the password change operation.</returns>

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.ChangePassword(form, _securityUtil.GetCurrentUserId());

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Gets the user account information for a given user ID
        /// </summary>
        /// <param name="userId">ID of the user account to retrieve information for</param>
        /// <returns>ServiceResult object containing the operation status and any relevant data</returns>

        [HttpGet("{userId:int:min(1)}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _userService.GetUserAsync(userId);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Action method for getting the current user's information.
        /// Requires an authenticated user.
        /// </summary>
        /// <returns>The current user's information.</returns>

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            ServiceResult result = await _userService.GetUserAsync(_securityUtil.GetCurrentUserId());

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Action method for updating the current user's information.
        /// Requires an authenticated user.
        /// </summary>
        /// <param name="form">The updated user information.</param>
        /// <returns>The updated user information.</returns>

        [Authorize]
        [HttpPut("edit")]
        public async Task<IActionResult> Update([FromBody] UserUpdateForm form)
        {
            if (form == null)
            {
                return BadRequest();
            }
            ServiceResult result = await _userService.EditAsync(_securityUtil.GetCurrentUserId(), form);

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }

    }
}
