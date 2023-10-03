using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Enums;

namespace RealEstateUser.Core.DTO.Views
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
        public int TotalBedrooms { get; set; }
        public int TotalBathrooms { get; set; }
        public byte CategoryType { get; set; }
        public string Status { get; set; }
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
            Latitude = property.Latitude;
            Longitude = property.Longitude;
            SquareFootage = property.SquareFootage;
            Status = ((PropertyStatus)property.Status).ToString();
            CategoryType = ((byte)((CategoryType)property.Category.Type));
            TotalBathrooms = property.TotalBathrooms;
            TotalBedrooms = property.TotalBedrooms;
        }

    }
    public class PropertyDetailView : PropertyView
    {

        public float MonthlyRent { get; set; }
        public float SecurityDeposit { get; set; }
        public float SalesPrice { get; set; }
        public byte? HideAddress { get; set; }

        public PropertyDetailView(Property property, PropertyAdditionalInfo? info, string? thumbnail) : base(property, thumbnail)
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
            HideAddress = info?.HideAddress;
        }
    }
}
