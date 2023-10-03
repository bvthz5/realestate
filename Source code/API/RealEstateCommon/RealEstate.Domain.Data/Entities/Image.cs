namespace RealEstate.Domain.Data.Entities
{
    public class Image
    {
        public int ImageId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public string PropertyImages { get; set; } = null!;
    }
}
