using Microsoft.Extensions.Logging;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<ImageService> _logger;

        private readonly ImageFileUtil _fileUtil;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public ImageService(IUnitOfWork unitOfWork, ILogger<ImageService> logger, ImageFileUtil fileUtil)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileUtil = fileUtil;
        }

        public async Task<ServiceResult> GetPhotosAsync(int propertyId)
        {
            ServiceResult result = new();
            try
            {
                if (await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId) == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"No image Found for this {propertyId}";
                    return result;
                }
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = await _unitOfWork.ImageRepository.FindByProductIdAsync(propertyId);
                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "ImS=01:SererError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;

        }
        public FileStream? GetPhotosByName(string fileName)
        {

            return _fileUtil.GetPropertyImages(fileName);

        }

        public FileStream? GetVideosByName(string fileName)
        {
            return _fileUtil.GetPropertyVideos(fileName);
        }
    }
}
