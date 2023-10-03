using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.DTO.Views
{
    public class UserView
    {
        public int UserId { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public string Email { get; }
        public byte Status { get; }
        public string? ProfilePic { get; set; }
        public DateTime CreatedOn { get; } 
        public int CreatedBy { get; }
        public UserView(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Status = user.Status;
            ProfilePic = user.ProfilePic;
            CreatedOn = user.CreatedOn;
        }
    }
    public class UserDetailView : UserView
    {
        public string? Address { get; }
        public string? PhoneNumber { get; }
        public DateTime UpdatedOn { get; }
        public int UpdatedBy { get; }
        public UserDetailView(User user) : base(user)
        {
            Address = user.Address;
            PhoneNumber = user.PhoneNumber;
            UpdatedOn = user.UpdatedOn;
            UpdatedBy = user.UpdatedBy ?? 0;
        }
    }
}
