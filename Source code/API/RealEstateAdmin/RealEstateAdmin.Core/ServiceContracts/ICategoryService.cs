using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using static RealEstateAdmin.Core.DTO.Forms.CategoryForm;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<ServiceResult> AddCategory(CategoryForm form);
        Task<ServiceResult> CategoryList();
        Task<ServiceResult> EditCategory(CategoryUpdateForm form, int categoryId);
    }
}
