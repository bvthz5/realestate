using System.ComponentModel.DataAnnotations;
using RealEstateUser.Core.DTO.Validations;
using PhoneAttribute = RealEstateUser.Core.DTO.Validations.PhoneAttribute;

namespace RealEstateUser.Core.DTO.Forms
{
    public class EnquiryTourForm
    {
        public int PropertyId { get; set; }
        [Date]
        public DateTime? AvailableDates { get; set; }
        [Time]
        public string? AvailableTime { get; set; }

        [Phone(Nullable = true)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        [RegularExpression("^\\S.*$", ErrorMessage = "Invalid message format. Please enter a non-empty message that does not start with a whitespace character.")]
        public string Message { get; set; } = string.Empty;
        public byte EnquiryType { get; set; }

        [StringLength(50)]
        [RegularExpression("^[^\\s].*[A-Za-z\\s]$", ErrorMessage = "Invalid name format. Please enter only letters.")]
        public string Name { get; set; } = null!;

    }
    public class EnquiryRequestForm
    {
        public int PropertyId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public byte Type { get; set; }

    }
}


