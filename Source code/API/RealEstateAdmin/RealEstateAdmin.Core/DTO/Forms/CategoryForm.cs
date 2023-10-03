using System.ComponentModel.DataAnnotations;

namespace RealEstateAdmin.Core.DTO.Forms
{
    public class CategoryForm
    {
        [Required(ErrorMessage = "Category name is required", AllowEmptyStrings = false)]
        [StringLength(20)]
        [RegularExpression("^[A-Za-z\\s]*$", ErrorMessage = "Only Alphabets and space allowed.")]
        public string CategoryName { get; set; } = null!;
        public byte Type { get; set; }
        public class CategoryUpdateForm : CategoryForm
        {
            [Required]
            public byte Status { get; set; }
        }
    }
}
