using Microsoft.AspNetCore.Http;
using RealEstateAdmin.Core.DTO.Validations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class PropertyImageForm
    {
        [Image]
        public IFormFile[]? File { get; set; }
    }
}
