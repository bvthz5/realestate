using Microsoft.AspNetCore.Http;
using RealEstateAdmin.Core.DTO.Validations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class PropertyVideoForm
    {
        [Video]
        public IFormFile[]? VideoFile { get; set; }
    }
}
