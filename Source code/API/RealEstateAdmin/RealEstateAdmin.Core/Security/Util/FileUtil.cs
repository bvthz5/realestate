using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Settings;

namespace RealEstateAdmin.Core.Security.Util
{
    public class FileUtil
    {
        private readonly ILogger<FileUtil> _logger;
        private readonly ImageSettings _imageSettings;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public FileUtil(IOptions<ImageSettings> imageSttings, ILogger<FileUtil> logger)
        {
            _imageSettings = imageSttings.Value;
            _logger = logger;
        }

        public string? UploadPropertyImage(Property property, IFormFile file)
        {
            try
            {
                string path = Path.Combine(_imageSettings.Path, _imageSettings.PropertyImagePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileInfo = new FileInfo(file.FileName);

                string fileName = $"{property.PropertyId}_{Guid.NewGuid()}{fileInfo.Extension}";

                if (UploadImage(path, fileName, file))
                    return fileName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }

            return null;
        }

        public FileStream? GetPropertyImages(string fileName)
        {
            try
            {
                var path = Path.Combine(_imageSettings.Path, _imageSettings.PropertyImagePath, fileName);
                return File.OpenRead(path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return null;
        }

        public bool DeletePropertyImages(string fileName)
        {
            var path = Path.Combine(_imageSettings.Path, _imageSettings.PropertyImagePath, fileName);

            return DeleteFile(path);
        }

        public string? UploadUserProfilePic(User user, IFormFile file)
        {
            try
            {
                string path = Path.Combine(_imageSettings.Path, _imageSettings.UserImagePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileInfo = new FileInfo(file.FileName);

                string fileName = $"{user.UserId}_{Guid.NewGuid()}{fileInfo.Extension}";

                if (UploadImage(path, fileName, file))
                    return fileName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }

            return null;
        }

        public FileStream? GetUserProfile(string fileName)
        {
            try
            {
                var path = Path.Combine(_imageSettings.Path, _imageSettings.UserImagePath, fileName);
                return File.OpenRead(path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return null;
        }

        public bool DeleteUserProfilePic(string fileName)
        {
            var path = Path.Combine(_imageSettings.Path, _imageSettings.UserImagePath, fileName);

            return DeleteFile(path);
        }

        private bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
                return false;
            }
        }

        private bool UploadImage(string path, string fileName, IFormFile file)
        {

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
                return false;
            }
        }

        public string? UploadPropertyVideo(Property property, IFormFile VideoFile)
        {
            try
            {
                string path = Path.Combine(_imageSettings.Path, _imageSettings.PropertyVideoPath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileInfo = new FileInfo(VideoFile.FileName);

                string fileName = $"{property.PropertyId}_{Guid.NewGuid()}{fileInfo.Extension}";

                if (UploadImage(path, fileName, VideoFile))
                    return fileName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }

            return null;
        }

        public FileStream? GetPropertyVideos(string fileName)
        {
            try
            {
                var path = Path.Combine(_imageSettings.Path, _imageSettings.PropertyVideoPath, fileName);
                return File.OpenRead(path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return null;
        }

        public bool DeletePropertyVideos(string fileName)
        {
            var path = Path.Combine(_imageSettings.Path, _imageSettings.PropertyVideoPath, fileName);

            return DeleteFile(path);
        }
    }
}
