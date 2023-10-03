using RealEstateAdmin.Core.DTO.Validations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class PropertyForm : PropertyAdditionalInfoForm
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = null!;
        [Required]
        [Address]
        public string Address { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string City { get; set; } = null!;
        [Required]
        [ZipCode]
        [MinLength(6)]
        public string ZipCode { get; set; } = null!;
        [Range(0.0, 10000000.00)]
        public float Price { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Longitude { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
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
    }
    public class PropertyAdditionalInfoForm
    {
        
        public string? PetPolicy { get; set; }
        public double? PetDeposit { get; set; }
        public double? PetRent { get; set; }
        public byte PetRateNegotiable { get; set; }
        public string? MyPropLeaseTermserty { get; set; }
        public double? LeaseDuration { get; set; }
        public string? Amenities { get; set; }
        public string? AvailableDaysToShow { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public byte AllowToContact { get; set; }
        public string? ContactNumber { get; set; }
        public byte HideAddress { get; set; }
        public string? UnitFeatures { get; set; }
        public string? SpecialFeatures { get; set; }
    }
}