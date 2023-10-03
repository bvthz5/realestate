using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Data.Entities
{
    public class Property : BaseEntity
    {
        public int PropertyId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Address { get; set; } = null!;

        [StringLength(50)]
        public string City { get; set; } = string.Empty!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public byte Status { get; set; }
        public float Price { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string ZipCode { get; set; } = string.Empty!;
        public int TotalBedrooms { get; set; }
        public int TotalBathrooms { get; set; }
        public float MonthlyRent { get; set; }
        public float SquareFootage { get; set; }
        public float SecurityDeposit { get; set; }
        public bool PropertyType { get; set; }
    }
}