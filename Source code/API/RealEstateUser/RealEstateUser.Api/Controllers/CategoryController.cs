using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets a list of active categories.
        /// </summary>
        /// <returns>A <see cref="ServiceResult"/> containing the list of active categories.</returns>

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetCategory()
        {
            if (ModelState.IsValid)
            {
                ServiceResult result = await _categoryService.CategoryList();

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
