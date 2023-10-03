using System.ComponentModel.DataAnnotations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class PropertyPaginationForm : PaginationSearchSortingForm
    {
        public string CategoryIds { get; set; } = string.Empty;

        [Range(0, 10000000)]
        public float StartPrice { get; set; } = 0;

        [Range(0, 10000000)]
        public float EndPrice { get; set; } = 0;

        [StringLength(6)]
        public string? ZipCode { get; set; }

        [StringLength(15)]
        public string Status { get; set; } = string.Empty;

        public byte CategoryType { get; set; }
        public int TotalBedrooms { get; set; }
        public int TotalBathrooms { get; set; }
    }
}

