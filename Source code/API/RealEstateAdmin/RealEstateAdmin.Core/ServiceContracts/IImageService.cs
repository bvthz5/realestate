using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IImageService
    {
        Task<ServiceResult> AddPhotosAsync(int propertyId, PropertyImageForm image, PropertyVideoForm video);
        Task<ServiceResult> GetPhotosAsync(int propertyId);
        Task<ServiceResult> DeletePhotosByPropertyIdAsync(int propertyId);
        Task<ServiceResult> DeletePhotosByPhotoIdAsync(int ImageId);
        FileStream? GetPhotosByName(string fileName);
        FileStream? GetVideosByName(string fileName);
    }
}
