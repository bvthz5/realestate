namespace RealEstate.Domain.Data.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? ProfilePic { get; set; }

        public string? VerificationCode { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
