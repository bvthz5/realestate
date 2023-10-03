using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RealEstateUser.Core.Security.Util
{


    public class SecurityUtil
    {
        private readonly ClaimsPrincipal? _user;

        // Add parameterless constructor for testing purposes
        public SecurityUtil()
        {
            _user = null;
        }

        public SecurityUtil(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext?.User;
        }

        /// <summary>
        /// For authenticated request returns userId from ClaimsPrincipal.<br/>
        /// Returns 0 if Not-Authenticated request
        /// </summary>
        /// <returns>Current UserId or 0 </returns>
        public int GetCurrentUserId()
        {
            var userId = _user?.FindFirst(ClaimTypes.Sid)?.Value;

            if (int.TryParse(userId, out int result))
            {
                return result;
            }

            return 0;
        }
    }

}

