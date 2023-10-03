using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface ICategoryService
    {
        Task<ServiceResult> CategoryList();
    }
}
