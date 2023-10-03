using System.ComponentModel.DataAnnotations;

namespace RealEstateUser.Core.DTO.Forms
{
    public class PaginationSearchSortingForm
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [StringLength(255)]
        public string? Search { get; set; }

        [StringLength(20)]
        public string? SortBy { get; set; }

        public bool SortByDesc { get; set; } = false;
    }
}

