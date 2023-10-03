namespace RealEstateAdmin.Core.DTO.Views
{
    public class LocationView
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public LocationView(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
