using Microsoft.AspNetCore.Mvc;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Api.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Gets the photos for the specified product.
        /// </summary>
        /// <param name="productId">The ID of the product to get photos for.</param>
        /// <returns>The status code and service result containing the photo data.</returns>

        [HttpGet("{PropertyId:int:min(1)}")]
        public async Task<IActionResult> GetPhotos(int PropertyId)
        {
            if (PropertyId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _imageService.GetPhotosAsync(PropertyId);

                // Return the appropriate HTTP status code and result.
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Gets the photo(s) for the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the photo(s) to get.</param>
        /// <returns>The file stream of the photo(s) with the specified file path.</returns>

        [HttpGet("{filePath}")]
        public IActionResult GetPhotosByPath(string filePath)
        {
            FileStream? fileStream = _imageService.GetPhotosByName(filePath);
            if (fileStream == null)
                return new NotFoundResult();

            return File(fileStream, "image/jpeg");
        }

        /// <summary>
        /// Gets the video(s) for the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the video(s) to get.</param>
        /// <returns>The file stream of the video(s) with the specified file path.</returns>

        [HttpGet("video/{filePath}")]
        public IActionResult GetVideoByName(string filePath)
        {
            FileStream? fileStream = _imageService.GetVideosByName(filePath);
            if (fileStream == null)
                return new NotFoundResult();

            return File(fileStream, "video/mp4");
        }
    }
}
