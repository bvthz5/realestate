using System.ComponentModel.DataAnnotations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class ProductPaginationParams : PaginationSearchSortingParams
    {
        public string CategoryIds { get; set; } = string.Empty;
        [StringLength(15)]
        public string Status { get; set; } = string.Empty;
    }
}
