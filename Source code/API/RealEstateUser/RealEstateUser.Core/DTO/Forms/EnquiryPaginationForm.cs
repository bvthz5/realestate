using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateUser.Core.DTO.Forms
{
    public class EnquiryPaginationForm : PaginationSearchSortingForm
    {
        public string Status { get; set; } = string.Empty;
        public byte EnquiryType { get; set; }

    }
}
