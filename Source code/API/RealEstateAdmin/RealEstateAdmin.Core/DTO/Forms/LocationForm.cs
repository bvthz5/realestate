using System.ComponentModel.DataAnnotations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class LocationForm
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
