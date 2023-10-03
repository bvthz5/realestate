using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;

namespace RealEstateAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;


        public ImageController(IImageService imageService)
        {
            _imageService = imageService;

        }

        /// <summary>
        /// Add Property Attachments
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <param name="image"></param>
        /// <param name="video"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add-new/property/{propertyId:int:min(1)}")]
        public async Task<IActionResult> Add(int propertyId, [FromForm] PropertyImageForm image, [FromForm] PropertyVideoForm video)
        {
            if (image == null)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _imageService.AddPhotosAsync(propertyId, image, video);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Get Property Photos
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        [HttpGet("property/{propertyId:int:min(1)}")]
        public async Task<IActionResult> GetPhotos(int propertyId)
        {
            if (propertyId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _imageService.GetPhotosAsync(propertyId);
                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Get Property Photos
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("image-path/{filePath}")]
        public IActionResult GetPhotosByPath(string filePath)
        {
            bool video = filePath.Split('.')[1].ToLower() == "mp4";
            FileStream? fileStream;
            if (video)
                fileStream = _imageService.GetVideosByName(filePath);
            else
                fileStream = _imageService.GetPhotosByName(filePath);

            if (fileStream == null)
                return new NotFoundResult();
            return File(fileStream, video ? "video/mp4" : "image/jpeg");
        }

        /// <summary>
        /// Delete Property Photos
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("property/{propertyId:int:min(1)}")]
        public async Task<IActionResult> DeletePhotos(int propertyId)
        {
            if (propertyId == 0)
            {
                return BadRequest();
            }
            else
            {
                ServiceResult result = await _imageService.DeletePhotosByPropertyIdAsync(propertyId);

                return StatusCode((int)result.ServiceStatus, result);
            }
        }

        /// <summary>
        /// Delete Photos By ID
        /// </summary>
        /// <param name=imageId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("image-id/{imageId:int:min(1)}")]
        public async Task<IActionResult> DeletePhotosByPhotoId(int imageId)
        {
            if (imageId == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok(await _imageService.DeletePhotosByPhotoIdAsync(imageId));
            }
        }
    }
}
