namespace RealEstate.Domain.Data.Entities
{
    public class PropertyUnits :BaseEntity
    {
        public int PropertyUnitsId { get; set; }

        public int PropertyId { get; set; }

        public Property Property { get; set; } = null!;

        public string UnitName { get; set; } = string.Empty;

        public bool AvailabilityStatus { get; set; }

        public DateTime AvailableFrom { get; set; }

        public int TotalBathrooms { get; set; }

        public int TotalBedrooms { get; set; }
        
        public float SquareFootage { get; set; }
        
        public float SecurityDeposit { get; set; }
        
        public float Price { get; set; }
        
        public string UnitFeatures { get; set; } = null!;
    }
}
