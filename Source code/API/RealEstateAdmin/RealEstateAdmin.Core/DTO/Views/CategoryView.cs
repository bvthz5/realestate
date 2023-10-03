using RealEstate.Domain.Data.Entities;

namespace RealEstateAdmin.Core.DTO.Views
{
    public class CategoryView
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public CategoryView(Category category)
        {
            CategoryId = category.CategoryId;
            CategoryName = category.CategoryName;
        }
        public CategoryView()
        {
            
        }
    }
    public class CategoryDetailView : CategoryView
    {
        public byte Status { get; set; }
        public IEnumerable<Category>? Category { get; }

        public CategoryDetailView(Category category) : base(category)
        {
            Status = category.Status;
        }

        public CategoryDetailView(IEnumerable<Category> category)
        {
            Status = 0;
            Category = category;
        }
    }

}
