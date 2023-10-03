using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;

namespace RealEstateAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

       /// <summary>
       /// Get Category
       /// </summary>
       /// <returns></returns>
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetCategory()
        {
            if (ModelState.IsValid)
            {
                ServiceResult result = await _categoryService.CategoryList();
                return StatusCode((int)result.ServiceStatus, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
