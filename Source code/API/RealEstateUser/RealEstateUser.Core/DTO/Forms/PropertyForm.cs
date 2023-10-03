using System.ComponentModel.DataAnnotations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class PropertyForm
    {

        [StringLength(1000)]
        [RegularExpression("^[A-Za-z0-9!@%&()_\\-,.\"'+\\n\\s]+$", ErrorMessage = "Invalid character Present")]
        public string? PropertyDescription { get; set; }

        public int CategoryId { get; set; }

        [Range(0.0, 10000000.00)]
        public float Price { get; set; }

        [Required]
        public LocationForm Location { get; set; } = null!;
    }
}
