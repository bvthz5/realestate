using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Enums;
using System.Reflection.Emit;

namespace RealEstateAdmin.Core.DTO.Views
{
    public class EnquiryView
    {
        public int EnquiryId { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public string CategoryType { get; set; }
        public string Property { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string? PhoneNumber { get; set; }
        public string Message { get; set; } 
        public string EnquiryType { get; set; }
        public DateTime? AvailableDates { get; set; }
        public string? AvailableTime { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public string Zipcode { get; set; }
        public double Price { get; set; }
        public string EnquiryStatus { get; set; }
        public string City { get; set; }
        public EnquiryView(Enquiry enquiry)
        {
            PropertyId = enquiry.Property.PropertyId;
            EnquiryId = enquiry.EnquiryId;
            UserId = enquiry.UserId;
            Property = enquiry.Property.Address;
            Name = enquiry.Name;
            CategoryType = enquiry.Property.Category.CategoryName;
            Description = enquiry.Property.Description;
            Zipcode = enquiry.Property.ZipCode;
            Price = enquiry.Property.Price;
            City = enquiry.Property.City;
            Email = enquiry.Email;
            PhoneNumber = enquiry.PhoneNumber;
            Message = enquiry.Message;
            Property = enquiry.Property.Address;
            EnquiryStatus = ((EnquiryStatus)enquiry.Status).ToString();
            EnquiryType = ((EnquiryType)enquiry.EnquiryType).ToString();
            AvailableDates = enquiry.AvailableDates;
            AvailableTime = enquiry.AvailableTime;
            Status = enquiry.Status;
        }
    }
}
