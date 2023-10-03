using Microsoft.AspNetCore.Http;
using RealEstateUser.Core.DTO.Validations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class ImageForm
    {
        [Image]
        public IFormFile File { get; set; } = null!;
    }
}
