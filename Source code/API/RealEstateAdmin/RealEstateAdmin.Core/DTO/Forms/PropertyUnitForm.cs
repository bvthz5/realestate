using System.ComponentModel.DataAnnotations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class PropertyUnitForm
    {
        [Required]
        [StringLength(100)]
        public string UnitName { get; set; } = null!;
        [Required]
        public bool AvailabilityStatus { get; set; } 
        [Required]
        public DateTime AvailableFrom { get; set; }
        [Required]
        [Range(0.0, 10000000.00)]
        public float Price { get; set; }
        [Required]
        [Range(0, 20)]
        public int TotalBedrooms { get; set; }
        [Required]
        [Range(0, 20)]
        public int TotalBathrooms { get; set; }
        [Required]
        [Range(0, 999999)]
        public float SquareFootage { get; set; }
        [Required]
        [Range(0, 1000000)]
        public float SecurityDeposit { get; set; }
        [Required]
        [StringLength(1000)]
        public string UnitFeatures { get; set; } = null!;

    }

}
