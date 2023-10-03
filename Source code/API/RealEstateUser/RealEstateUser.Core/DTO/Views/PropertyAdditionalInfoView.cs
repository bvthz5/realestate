using RealEstate.Domain.Data.Entities;

namespace RealEstateUser.Core.DTO.Views
{
    public class PropertyAdditionalInfoView : PropertyDetailView
    {
        public int? PropertyAdditionalInfoId { get; set; }
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
        public string? UnitFeatures { get; set; }
        public string? SpecialFeatures { get; set; }

        public PropertyAdditionalInfoView(PropertyAdditionalInfo? propertyAdditionalInfo, Property property, string? thumbnail) : base(property, propertyAdditionalInfo, thumbnail)
        {
            PropertyAdditionalInfoId = propertyAdditionalInfo?.PropertyAdditionalInfoId;
            PetPolicy = propertyAdditionalInfo?.PetPolicy;
            PetDeposit = propertyAdditionalInfo?.PetDeposit;
            PetDeposit = propertyAdditionalInfo?.PetDeposit;
            PetRateNegotiable = propertyAdditionalInfo?.PetRateNegotiable ?? 0;
            LeaseDuration = propertyAdditionalInfo?.LeaseDuration;
            Amenities = propertyAdditionalInfo?.Amenities;
            AvailableDaysToShow = propertyAdditionalInfo?.AvailableDaysToShow;
            AllowToContact = propertyAdditionalInfo?.AllowToContact ?? 0;
            AvailableFrom = propertyAdditionalInfo?.AvailableFrom;
            ContactNumber = propertyAdditionalInfo?.ContactNumber;
            UnitFeatures = propertyAdditionalInfo?.UnitFeatures;
            SpecialFeatures = propertyAdditionalInfo?.SpecialFeatures;
            PetRent = propertyAdditionalInfo?.PetRent;
            MyPropLeaseTermserty = propertyAdditionalInfo?.MyPropLeaseTermserty;
        }
    }
}
