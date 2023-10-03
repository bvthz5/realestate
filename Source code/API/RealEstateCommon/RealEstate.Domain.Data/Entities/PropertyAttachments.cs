namespace RealEstate.Domain.Data.Entities
{
    public class PropertyAttachments : BaseEntity
    {
        public int PropertyAttachmentsId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public string AttachmentPath { get; set; } = null!;
    }
}
