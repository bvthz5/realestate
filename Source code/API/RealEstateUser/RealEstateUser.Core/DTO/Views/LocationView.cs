namespace RealEstateUser.Core.DTO.Views
{
    public class LocationView
    {
        public string? Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }


        public string ZipCode { get; set; }

        public LocationView(string? addess, double latitude, double longitude, string zipCode)
        {
            Address = addess;
            Latitude = latitude;
            Longitude = longitude;
            ZipCode = zipCode;
        }
    }
}
