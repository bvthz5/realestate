using Microsoft.AspNetCore.Mvc;
using RealEstateAdmin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAdmin.Core.ServiceContracts
{
    public interface IUserService
    {

        Task<ServiceResult> GetUserAsync(int userId);
        Task<ServiceResult> GetUserList(string? searchQuery, string? sortBy, bool isSortAscending);
        Task<ServiceResult> Count();
        Task<FileStream?> GetProfile(string fileName);
        Task<ServiceResult> ChangeStatusAsync(int userId, byte status);
    }
}
