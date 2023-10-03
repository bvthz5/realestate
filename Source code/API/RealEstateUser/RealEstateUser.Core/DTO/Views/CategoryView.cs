using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Enums;

namespace RealEstateUser.Core.DTO.Views
{
    public class CategoryView
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public byte CategoryType { get; set; }

        public CategoryView(Category category)
        {
            CategoryId = category.CategoryId;

            CategoryName = category.CategoryName;

            CategoryType = ((byte)(CategoryType)category.Type);

        }
    }

    public class CategoryDetailView : CategoryView
    {
        public byte Status { get; set; }

        public CategoryDetailView(Category category) : base(category)
        {
            Status = ((byte)((CategoryStatus)category.Status));
        }
    }
}
