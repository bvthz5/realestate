using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly SecurityUtil _securityUtil;

        public WishListController(IWishListService wishListService, SecurityUtil securityUtil)
        {
            _wishListService = wishListService;
            _securityUtil = securityUtil;
        }

        /// <summary>
        /// Adds a product to the user's wishlist/favourite.
        /// </summary>
        /// <param name="productId">The ID of the product to add.</param>
        /// <returns>A <see cref="ServiceResult"/> indicating the result of the operation.</returns>

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] int propertyId)
        {
            if (propertyId == 0)
            {
                return BadRequest();
            }
            ServiceResult result = await _wishListService.AddToWishList(_securityUtil.GetCurrentUserId(), propertyId);

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Removes a product from the user's wishlist/favourite.
        /// </summary>
        /// <param name="productId">The ID of the product to remove.</param>
        /// <returns>A <see cref="ServiceResult"/> indicating the result of the operation.</returns>

        [Authorize]
        [HttpDelete("{propertyId:int:min(1)}")]
        public async Task<IActionResult> Delete(int propertyId)
        {
            if (propertyId == 0)
            {
                return BadRequest();
            }
            ServiceResult result = await _wishListService.RemoveFromWishList(_securityUtil.GetCurrentUserId(), propertyId);

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }

        /// <summary>
        /// Gets the user's wishlist/favourite.
        /// </summary>
        /// <returns>A <see cref="ServiceResult"/> containing the user's wish list.</returns>

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _wishListService.GetWishList(_securityUtil.GetCurrentUserId());

            // Return the appropriate HTTP status code and result.
            return StatusCode((int)result.ServiceStatus, result);
        }
    }
}
