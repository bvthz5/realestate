using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Enums;

namespace RealEstateUser.Core.DTO.Views
{
    public class EnquiryTourView
    {
        public  string? Thumbnail { get; }

        public DateTime? AvailableDates { get; set; }
        public string? AvailableTime { get; set; }
        public string Name { get; set; } 
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Message { get; set; }
        public byte EnquiryType { get; set; }
        public byte Status { get; set; }
        public string Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PropertyStatus { get; set; }

        public EnquiryTourView(Enquiry enquiry)
        {
            AvailableDates = enquiry.AvailableDates;
            AvailableTime = enquiry.AvailableTime;
            Name = enquiry.Name;
            Phone = enquiry.PhoneNumber;
            Email = enquiry.Email;
            Message = enquiry.Message;
            EnquiryType = ((byte)((EnquiryType)enquiry.EnquiryType));
            Status = ((byte)((EnquiryStatus)enquiry.Status));
            Address = enquiry.Property.Address;
            CreatedOn = enquiry.CreatedOn.ToUniversalTime();
            PropertyStatus = enquiry.Property.Status.ToString();
        }

        public EnquiryTourView(Enquiry enquiry, string? thumbnail) : this(enquiry)
        {
            Thumbnail = thumbnail;
        }
    }

    public class RequestDetailview : EnquiryTourView
    {
        public int EnquiryId { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public byte CategoryType { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? Zipcode { get; set; }
        public float Price { get; set; }
        public string? City { get; set; }
        public new byte PropertyStatus { get; set; }


        public RequestDetailview(Enquiry enquiry) : base(enquiry)
        {
            EnquiryId = enquiry.EnquiryId;
            UserId = enquiry.UserId;
            PropertyId = enquiry.PropertyId;

            CategoryName = enquiry.Property.Category.CategoryName;
            CategoryType = ((byte)((CategoryType)enquiry.Property.Category.Type));

            Description = enquiry.Property.Description;
            Address = enquiry.Property.Address;
            Zipcode = enquiry.Property.ZipCode;
            Price = enquiry.Property.Price;
            City = enquiry.Property.City;
            PropertyStatus = ((byte)((PropertyStatus)enquiry.Property.Status));

            AvailableDates = enquiry.AvailableDates;
            AvailableTime = enquiry.AvailableTime;

            Name = enquiry.Name;
            Phone = enquiry.PhoneNumber;
            Email = enquiry.Email;

            Message = enquiry.Message;
            EnquiryType = ((byte)((EnquiryType)enquiry.EnquiryType));
            Status = ((byte)((EnquiryStatus)enquiry.Status));

            CreatedOn = enquiry.CreatedOn;


        }
    }
}


