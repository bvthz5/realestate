namespace RealEstate.Domain.Data.Entities
{
    public class WishList : BaseEntity
    {
        public int WishListId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
