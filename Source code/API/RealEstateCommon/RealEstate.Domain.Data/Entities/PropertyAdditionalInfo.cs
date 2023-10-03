namespace RealEstate.Domain.Data.Entities
{
    public class PropertyAdditionalInfo
    {
        public int PropertyAdditionalInfoId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
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
