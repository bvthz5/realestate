using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Data.Entities
{
    public class Category : BaseEntity
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; } = null!;

        [Required]
        public byte Type { get; set; }

        [Required]
        public byte Status { get; set; }
    }
}
