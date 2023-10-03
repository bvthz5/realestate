using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.Security.Util;
using RealEstateAdmin.Core.ServiceContracts;
using Property = RealEstate.Domain.Data.Entities.Property;

namespace RealEstateAdmin.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUtil _fileUtil;
        private readonly ILogger<ImageService> _logger;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";
        public ImageService(IUnitOfWork unitOfWork, FileUtil fileUtil, ILogger<ImageService> logger)
        {
            _unitOfWork = unitOfWork;
            _fileUtil = fileUtil;
            _logger = logger;
        }

        /// <summary>
        /// Add Photo
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="image"></param>
        /// <param name="video"></param>
        /// <returns></returns>
        public async Task<ServiceResult> AddPhotosAsync(int propertyId, PropertyImageForm image, PropertyVideoForm video)
        {
            ServiceResult result = new();

            if ((image.File is null || image.File.Length == 0) && (video.VideoFile is null || video.VideoFile.Length == 0))
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "Minimum 1 Image or Video Required";
                return result;
            }

            Property? property = await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId);


            int imageCount = (await _unitOfWork.ImageRepository.FindByPropertyIdAsync(propertyId))
                .Where(file => !file.PropertyImages.ToLower().EndsWith(".mp4"))
                .ToList().Count + image.File?.Length ?? 0;
            if (imageCount > 10)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "Image count 10 exceeded";
                return result;
            }

            int videoCount = (await _unitOfWork.ImageRepository.FindByPropertyIdAsync(propertyId))
                .Where(file => file.PropertyImages.ToLower().EndsWith(".mp4"))
                .ToList().Count + video.VideoFile?.Length ?? 0;
            if (videoCount > 2)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "Video count 2 exceeded";
                return result;
            }

            if (image.File?.Length > 0)
            {
                foreach (var imageItem in image.File)
                {
                    var fileName = _fileUtil.UploadPropertyImage(property, imageItem) ?? throw new Exception("Image Not Uploaded");

                    _unitOfWork.PropertyRepository.AddImage(new Image()
                    {
                        Property = property,
                        PropertyImages = fileName
                    });
                }
            }

            if (video.VideoFile?.Length > 0)
            {
                foreach (var videoItem in video.VideoFile)
                {
                    var videoFileName = _fileUtil.UploadPropertyVideo(property, videoItem) ?? throw new Exception("Video Not Uploaded");
                    _unitOfWork.PropertyRepository.AddImage(new Image()
                    {
                        Property = property,
                        PropertyImages = videoFileName
                    });
                    property = _unitOfWork.PropertyRepository.Update(property);
                }
            }

            await _unitOfWork.SaveAsync();
            result.ServiceStatus = ServiceStatus.Success;
            result.Message = "Files Added";
            return result;
        }


        /// <summary>
        /// Get Photo
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> GetPhotosAsync(int propertyId)
        {
            ServiceResult result = new();
            try
            {
                var property = await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId);
                //var properties = (await _unitOfWork.PropertyRepository.Find(property => property.Status != (byte)PropertyStatus.Deleted));
                if (await _unitOfWork.ImageRepository.FindByPropertyIdAsync(propertyId) == null)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "No Image for the property";
                    return result;

                }
                if (property == null || property.Status == (byte)PropertyStatus.Deleted)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"Property Does Not Exist";
                    return result;
                }
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = await _unitOfWork.ImageRepository.FindByPropertyIdAsync(propertyId);
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "IS-02 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;

        }


        /// <summary>
        /// Delete Photo
        /// </summary>
        /// <param name="ImageId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeletePhotosByPhotoIdAsync(int ImageId)
        {
            ServiceResult result = new();
            try
            {
                var image = await _unitOfWork.ImageRepository.FindById(ImageId);

                if (image == null || image.Property.Status == (byte)PropertyStatus.Deleted || image.Property.Status == (byte)PropertyStatus.SoldOut)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Not Found";

                    return result;
                }
                Property? property = await _unitOfWork.PropertyRepository.FindByIdAsync(image.PropertyId);
                var images = await _unitOfWork.ImageRepository.FindByPropertyIdAsync(image.PropertyId);
                if (property != null && images.Count <= 1)
                {
                    _unitOfWork.PropertyRepository.Update(property);
                }
                _unitOfWork.ImageRepository.DeleteImage(image);
                _fileUtil.DeletePropertyImages(image.PropertyImages);
                await _unitOfWork.SaveAsync();
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = "Image Deleted";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "IS-03 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Delete Photo 
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeletePhotosByPropertyIdAsync(int propertyId)
        {
            ServiceResult result = new();
            try
            {
                var images = await _unitOfWork.ImageRepository.FindByPropertyIdAsync(propertyId);
                Property? property = await _unitOfWork.PropertyRepository.FindByIdAsync(propertyId);
                if (images.Count == 0 || property?.Status == (byte)PropertyStatus.Deleted || property?.Status == (byte)PropertyStatus.SoldOut)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Property Not Found";
                    return result;
                }
                foreach (var image in images)
                    _fileUtil.DeletePropertyImages(image.PropertyImages);
                if (property != null) // add null check
                    _unitOfWork.PropertyRepository.Update(property);
                _unitOfWork.ImageRepository.DeleteImages(images);
                await _unitOfWork.SaveAsync();
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = "Photos Deleted";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "IS-04 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }


        /// <summary>
        /// Get Photos
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileStream? GetPhotosByName(string fileName)
        {
            return _fileUtil.GetPropertyImages(fileName);
        }

        /// <summary>
        /// Get Videos
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileStream? GetVideosByName(string fileName)
        {
            return _fileUtil.GetPropertyVideos(fileName);
        }
    }
}
