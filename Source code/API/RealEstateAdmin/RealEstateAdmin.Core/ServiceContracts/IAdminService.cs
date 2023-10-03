using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IAdminService
    {
        Task<ServiceResult> Login(LoginForm form);
        Task<ServiceResult> RefreshAsync(string token);
        
    }
}
