using RealEstateUser.Core.Helpers;

namespace RealEstateUser.Core.ServiceContracts
{
    public interface IGoogleService
    {
        Task<ServiceResult> Login(string idToken);
    }
}
