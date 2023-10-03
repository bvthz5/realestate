using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Settings;

namespace RealEstateUser.Core.Security.Util
{
    public class ImageFileUtil
    {
        private readonly ILogger<ImageFileUtil> _logger;
        private readonly ImageSettings _imageSettings;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public ImageFileUtil(ILogger<ImageFileUtil> logger, IOptions<ImageSettings> imageSettings)
        {
            _logger = logger;
            _imageSettings = imageSettings.Value;
        }

        public string? UploadUserProfiePic(User user, IFormFile file)
        {
            try
            {
                string path = Path.Combine(_imageSettings.Path, _imageSettings.UserImagePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileinfo = new FileInfo(file.FileName);

                string fileName = $"{user.UserId}_{Guid.NewGuid()}{fileinfo.Extension}";

                if (UploadImage(path, fileName, file))
                    return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return null;
        }

        public FileStream? GetUserProfilePic(string fileName)
        {
            try
            {
                var path = Path.Combine(_imageSettings.Path, _imageSettings.UserImagePath, fileName);
                return System.IO.File.OpenRead(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return null;
        }

        public bool DeleteUserProfiePic(string fileName)
        {
            try
            {
                string path = Path.Combine(_imageSettings.Path, _imageSettings.UserImagePath, fileName);
                return DeleteFile(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
                return false;
            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
                return false;
            }
        }

        public bool UploadImage(string path, string fileName, IFormFile file)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNamePath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNamePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
                return false;
            }
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
    }
}
