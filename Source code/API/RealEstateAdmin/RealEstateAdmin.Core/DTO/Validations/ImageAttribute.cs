using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateAdmin.Core.DTO.Validations
{
    public class ImageAttribute : ValidationAttribute
    {
        private readonly string[] _acceptedFileTypes = { ".jpg", ".jpeg", ".png", ".webp" };

        private readonly int _maxBytes = 2 * 1240 * 1240;

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is IFormFile image)
            {
                if (!_acceptedFileTypes.Contains(Path.GetExtension(image.FileName)))
                {
                    ErrorMessage = $"Only {string.Join(", ", _acceptedFileTypes)} are Supported, Uploaded : '{Path.GetExtension(image.FileName)}'";
                    return false;
                }

                if (_maxBytes <= image.Length)
                {
                    ErrorMessage = $"Max File Size : {_maxBytes / 1240}KB, Uploaded File Size : {image.Length / 1240}KB ";
                    return false;
                }
                return true;
            }
            else if (value is IFormFile[] images)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    var file = images[i];
                    if (!_acceptedFileTypes.Contains(Path.GetExtension(file.FileName)))
                    {
                        ErrorMessage = $"Only {string.Join(", ", _acceptedFileTypes)} are Supported, Uploaded : '{Path.GetExtension(file.FileName)}'";
                        return false;
                    }

                    if (_maxBytes <= file.Length)
                    {
                        ErrorMessage = $"Max File Size : {_maxBytes / 1240}KB, Uploaded File Size : {file.Length / 1240}KB ";
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
