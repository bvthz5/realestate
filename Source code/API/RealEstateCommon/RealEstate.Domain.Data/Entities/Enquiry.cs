using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Data.Entities
{
    public class Enquiry : BaseEntity
    {
        public int EnquiryId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        [StringLength(50)]
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        [StringLength(13)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string Message { get; set; } = string.Empty;
        public byte EnquiryType { get; set; }
        public DateTime? AvailableDates { get; set; }
        public string? AvailableTime { get; set; }
        public byte Status { get; set; }
    }
}

