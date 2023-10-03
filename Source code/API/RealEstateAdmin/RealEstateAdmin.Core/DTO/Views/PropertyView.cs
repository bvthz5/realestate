using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Enums;

namespace RealEstateAdmin.Core.DTO.Views
{
    public class PropertyView
    {
        public int PropertyId { get; set; }
        public int CategoryId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CategoryName { get; set; }
        public string? Thumbnail { get; set; }
        public string ZipCode { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryType { get; set; }
        public string StatusValue { get; set; }
        public byte Status { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public float SquareFootage { get; set; }
        public PropertyView(Property property, string? thumbnail)
        {
            PropertyId = property.PropertyId;
            CategoryId = property.CategoryId;
            Address = property.Address;
            City = property.City;
            CategoryName = property.Category.CategoryName;
            ZipCode = property.ZipCode;
            Thumbnail = thumbnail;
            Description = property.Description;
            Price = property.Price;
            CreatedDate = property.CreatedOn;
            SquareFootage = property.SquareFootage;
            Status = property.Status;
            StatusValue = ((PropertyStatus)property.Status).ToString();
            CategoryType = ((CategoryType)property.Category.Type).ToString();
        }
    }
    public class PropertyDetailView : PropertyView
    {
        public int TotalBedrooms { get; set; }
        public int TotalBathrooms { get; set; }
        public float MonthlyRent { get; set; }
        public float SecurityDeposit { get; set; }
        public float SalesPrice { get; set; }
        public PropertyDetailView(Property property) : base(property, null)
        {
            PropertyId = property.PropertyId;
            CategoryId = property.CategoryId;
            Address = property.Address;
            City = property.City;
            ZipCode = property.ZipCode;
            Description = property.Description;
            Price = property.Price;
            CreatedDate = property.CreatedOn;
            Latitude = property.Latitude;
            Longitude = property.Longitude;
            TotalBedrooms = property.TotalBedrooms;
            TotalBathrooms = property.TotalBathrooms;
            MonthlyRent = property.MonthlyRent;
            SecurityDeposit = property.SecurityDeposit;
        }
    }
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
        public byte HideAddress { get; set; }
        public string? UnitFeatures { get; set; }
        public string? SpecialFeatures { get; set; }

        public PropertyAdditionalInfoView(PropertyAdditionalInfo? propertyAdditionalInfo, Property property) : base(property)
        {
            PropertyAdditionalInfoId = propertyAdditionalInfo?.PropertyAdditionalInfoId;
            PetPolicy = propertyAdditionalInfo?.PetPolicy;
            PetRent = propertyAdditionalInfo?.PetRent;
            PetDeposit = propertyAdditionalInfo?.PetDeposit;
            PetDeposit = propertyAdditionalInfo?.PetDeposit;
            PetRateNegotiable = propertyAdditionalInfo?.PetRateNegotiable ?? 0;
            LeaseDuration = propertyAdditionalInfo?.LeaseDuration;
            Amenities = propertyAdditionalInfo?.Amenities;
            AvailableDaysToShow = propertyAdditionalInfo?.AvailableDaysToShow;
            AllowToContact = propertyAdditionalInfo?.AllowToContact ?? 0;
            AvailableFrom = propertyAdditionalInfo?.AvailableFrom;
            ContactNumber = propertyAdditionalInfo?.ContactNumber;
            HideAddress = propertyAdditionalInfo?.HideAddress ?? 0;
            UnitFeatures = propertyAdditionalInfo?.UnitFeatures;
            SpecialFeatures = propertyAdditionalInfo?.SpecialFeatures;
        }
    }
}
