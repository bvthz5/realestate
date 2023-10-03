using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateAdmin.Core.DTO.Validations
{
    public class VideoAttribute : ValidationAttribute
    {
        private readonly string[] _acceptedFileTypes = { ".mp4" };

        private readonly int _maxBytes = 4 * 4096 * 2160;

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is IFormFile video)
            {
                if (!_acceptedFileTypes.Contains(Path.GetExtension(video.FileName)))
                {
                    ErrorMessage = $"Only {string.Join(", ", _acceptedFileTypes)} are Supported, Uploaded : '{Path.GetExtension(video.FileName)}'";
                    return false;
                }

                if (_maxBytes <= video.Length)
                {
                    ErrorMessage = $"Max File Size : {_maxBytes / 4096}KB, Uploaded File Size : {video.Length / 4096}KB ";
                    return false;
                }
                return true;
            }
            else if (value is IFormFile[] videos)
            {
                for (int i = 0; i < videos.Length; i++)
                {
                    var file = videos[i];
                    if (!_acceptedFileTypes.Contains(Path.GetExtension(file.FileName)))
                    {
                        ErrorMessage = $"Only {string.Join(", ", _acceptedFileTypes)} are Supported, Uploaded : '{Path.GetExtension(file.FileName)}'";
                        return false;
                    }

                    if (_maxBytes <= file.Length)
                    {
                        ErrorMessage = $"Max File Size : {_maxBytes / 4096}KB, Uploaded File Size : {file.Length / 4096}KB ";
                        return false;
                    }
                }
                return true;
            }

            ErrorMessage = "Invalid Type";
            return false;
        }
    }
}
