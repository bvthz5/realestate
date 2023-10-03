using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RealEstateAdmin.Core.Security.Util
{
    public class SecurityUtil
    {
        private readonly ClaimsPrincipal _users;
        public SecurityUtil(IHttpContextAccessor httpContextAccessor)
        {
            _users = httpContextAccessor.HttpContext.User;
        }
        /// <summary>
        /// For authenticated request returns userId from ClaimsPrincipal.<br/>
        /// Returns 0 if Not-Authenticated request
        /// </summary>
        /// <returns>Current UserId or 0 </returns>
        public int GetCurrentUserId()
        {
            var userId = _users.FindFirst(ClaimTypes.Sid);

            if (userId != null)
                return int.Parse(userId.Value);

            return 0;
        }
    }
}
