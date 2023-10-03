using Microsoft.Extensions.Logging;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.DTO.Views;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Core.Services
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
        /// Retrieves a list of all active categories.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result is a <see cref="ServiceResult"/> object containing the list of active categories.</returns>
        public async Task<ServiceResult> CategoryList()
        {
            ServiceResult result = new();
            try
            {
                result.Data = (await _unitOfWork.CategoryRepository.FindAllByStatusAsync((byte)CategoryStatus.Active)).ConvertAll(category => new CategoryView(category));
                result.ServiceStatus = ServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "CS=01: ServerError";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);

            }
            return result;
        }
    }
}

