using Microsoft.Extensions.Logging;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.Security.Util;
using RealEstateAdmin.Core.Security;
using RealEstateAdmin.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Domain.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateAdmin.Core.Services
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<AdminService> _logger;
        private readonly FileUtil _fileUtil;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public UserService(IUnitOfWork unitOfWork, FileUtil fileUtil, IEmailService emailService, ILogger<AdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailService = emailService;
            _fileUtil = fileUtil;
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> GetUserAsync(int userId)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User Not Found for Id : {userId}";
                    return result;
                }
                if (user.Status == (byte)(UserStatus.Deleted))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Deleted";
                    return result;
                }
                result.Data = new UserDetailView(user);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-01 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Change User Status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ChangeStatusAsync(int userId, byte status)
        {
            ServiceResult result = new();
            try
            {
                User? user = await _unitOfWork.UserRepository.FindByIdAsync(userId);
                string? statusValue = Enum.GetName(typeof(UserStatus), status);
                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }
                if (user.Status == status)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "User Already in " + statusValue + " Status";
                    return result;
                }
                if (status != (byte)UserStatus.Active && status != (byte)UserStatus.Blocked && status != (byte)UserStatus.Deleted && status != (byte)UserStatus.Inactive)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Status";
                    return result;
                }

                if (user.Status == status && status == (byte)UserStatus.Deleted)
                {
                    result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                    result.Message = "User Already deleted";
                    return result;
                }

                if (status == (byte)UserStatus.Deleted)
                {
                    user.Email = DateTime.Now + "_" + user.Email;
                }

                user.Status = status;
                user.UpdatedOn = DateTime.Now;
                _unitOfWork.AdminRepository.Update(user);
                await _unitOfWork.SaveAsync();
                string? name = user.FirstName + " " + user.LastName;
                _emailService.ChangeStatusUser(user.Email, statusValue, name);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "User Status Changed Successfully";
                result.Data = new UserDetailView(user);
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-02 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        public async Task<FileStream?> GetProfile(string fileName)
        {
            // Check if the profile picture exists in the file system.
            if (!await _unitOfWork.UserRepository.IsProfilePicExists(fileName))
                return null;

            // If the profile picture exists, retrieve it using the file utility.
            return _fileUtil.GetUserProfile(fileName);
        }

        /// <summary>
        /// Get User Count
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult> Count()
        {
            ServiceResult result = new();
            try
            {
                List<User> users = (await _unitOfWork.UserRepository.Find(users => users.Status != (byte)UserStatus.Deleted && users.Status !=(byte)UserStatus.Inactive)).ToList();
                if (users.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No Users Found";
                    return result;
                }
                int totalUsers = users.Count;
                int activeUsers = users.Count(user => user.Status == (byte)UserStatus.Active);
                int blockedUsers = users.Count(user => user.Status == (byte)UserStatus.Blocked);
                int inactiveUsers = users.Count(user => user.Status == (byte)UserStatus.Inactive);
                var userCounts = new Dictionary<string, int>()
                {
                    { "activeUsers", activeUsers },
                    { "blockedUsers", blockedUsers },
                    { "totalUsers", totalUsers },
                    { "inactiveUIsers", inactiveUsers },
                };

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = userCounts;
                result.Message = "Users Counts";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-03 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get Users List
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sortBy"></param>
        /// <param name="isSortAscending"></param>
        /// <returns></returns>
        public async Task<ServiceResult> GetUserList(string? searchQuery, string? sortBy, bool isSortAscending)
        {
            ServiceResult result = new();
            try
            {
                List<User> users = (List<User>)await _unitOfWork.UserRepository.Find(users => users.Status != (byte)UserStatus.Deleted && users.Status !=(byte)UserStatus.Inactive);
                users = FilterUsers(users, searchQuery);
                users = SortUsers(users, sortBy, isSortAscending);
                if (users.Count == 0)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "No Users Found";
                    return result;
                }
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = users.ConvertAll(user => new UserDetailView(user));
                result.Message = $"Success: Listed";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-04 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        private static List<User> FilterUsers(List<User> users, string? searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return users.Where(user =>
                    user.Status == (byte)UserStatus.Active || user.Status == (byte)UserStatus.Blocked || user.Status == (byte)UserStatus.Inactive)
                    .OrderByDescending(user => user.CreatedOn).ToList();
            }
            else
            {
                string searchQueryLower = searchQuery.ToLower();
                return users.Where(user =>
                    (user.FirstName + " " + user.Email).ToLower().Contains(searchQueryLower) &&
                    (user.Status == (byte)UserStatus.Active || user.Status == (byte)UserStatus.Blocked || user.Status == (byte)UserStatus.Inactive))
                    .ToList();
            }
        }

        private static List<User> SortUsers(List<User> users, string? sortBy, bool isSortAscending)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return users;
            }
            else
            {
                switch (sortBy.ToLower())
                {
                    case "firstname":
                        return isSortAscending ? users.OrderBy(user => user.FirstName).ToList()
                                               : users.OrderByDescending(user => user.FirstName)
                                                      .ThenByDescending(user => user.LastName).ToList();
                    case "lastname":
                        return isSortAscending ? users.OrderBy(user => user.LastName).ToList()
                                               : users.OrderByDescending(user => user.LastName)
                                                      .ThenByDescending(user => user.FirstName).ToList();
                    case "email":
                        return isSortAscending ? users.OrderBy(user => user.Email).ToList()
                                               : users.OrderByDescending(user => user.Email).ToList();
                    default:
                        return users;
                }
            }
        }
    }
}
