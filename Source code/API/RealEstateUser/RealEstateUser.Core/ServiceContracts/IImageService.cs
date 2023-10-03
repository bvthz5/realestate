using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface IImageService
    {
        Task<ServiceResult> GetPhotosAsync(int propertyId);
        FileStream? GetPhotosByName(string fileName);
        FileStream? GetVideosByName(string fileName);
    }
}
