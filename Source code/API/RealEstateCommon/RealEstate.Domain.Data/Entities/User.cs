using System.ComponentModel.DataAnnotations;


namespace RealEstate.Domain.Data.Entities
{
    public class User : BaseEntity
    {


        public int UserId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? LastName { get; set; }
        [Required]
        public string Email { get; set; } = null!;

        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public byte Status { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [StringLength(13)]
        public string? PhoneNumber { get; set; }

        public string? ProfilePic { get; set; }

        [StringLength(255)]
        public string? VerificationCode { get; set; }

    }
}
