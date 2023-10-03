using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;
using static RealEstateAdmin.Core.DTO.Forms.CategoryForm;

namespace RealEstateAdmin.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";
        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<ServiceResult> AddCategory(CategoryForm form)
        {
            ServiceResult result = new();
            try  ///category => category.CategoryName.ToLower() == form.CategoryName.Trim()) != null)
            {
                var categories = (await _unitOfWork.CategoryRepository.Find(category => category.CategoryName.ToLower() == form.CategoryName.Trim())).SingleOrDefault();
                if (form.Type != (byte)CategoryType.Buy && form.Type != (byte)CategoryType.Rent)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Type";
                    return result;
                }
                if (categories != null)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Category Already Exists";
                    return result;
                }
                var category = await _unitOfWork.CategoryRepository.Add(new()
                {
                    CategoryName = form.CategoryName.Trim(),
                    Type = form.Type,
                    Status = (byte)CategoryStatus.Active
                });
                await _unitOfWork.SaveAsync();
                result.Data = new CategoryDetailView(category);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Category Added Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "CS-01 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get Category List
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult> CategoryList()
        {
            ServiceResult result = new();
            try
            {
                var Category = ((await _unitOfWork.CategoryRepository.Find(category => category.Status == (byte)CategoryStatus.Active))
                    .OrderBy(Category => Category.CategoryName).ToList()).ConvertAll(category => new CategoryView(category));
                var category = await _unitOfWork.CategoryRepository.GetAll();
                if (Category != null)
                {
                    result.ServiceStatus = ServiceStatus.Success;
                    result.Data = new CategoryDetailView(category);
                    result.Message = $"Success: Listed";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "CS-02 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Edit Category
        /// </summary>
        /// <param name="form"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> EditCategory(CategoryUpdateForm form, int categoryId)
        {
            ServiceResult result = new();
            try
            {
                Category? category = await _unitOfWork.CategoryRepository.FindByCategoryNameAsync(form.CategoryName.Trim());
                if (category != null && category.CategoryId != categoryId)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "Category Already Exists";
                    return result;
                }
                category = await _unitOfWork.CategoryRepository.FindByIdAndStatusAsync(categoryId, (byte)CategoryStatus.Active);
                if (category == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "Category Not Found";
                    return result;
                }
                if (form.Status != (byte)CategoryStatus.Active && form.Status != (byte)CategoryStatus.Inactive)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Status";
                    return result;
                }
                category.CategoryName = form.CategoryName.Trim();
                category.Type = form.Type;
                category.Status = form.Status;
                _unitOfWork.CategoryRepository.Update(category);
                await _unitOfWork.SaveAsync();
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Category Updated";
                result.Data = new CategoryDetailView(category);
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "CS-03 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }
    }
}
