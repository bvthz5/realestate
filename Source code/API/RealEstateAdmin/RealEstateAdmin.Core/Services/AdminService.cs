using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.DTO.Views;
using RealEstateAdmin.Core.Enums;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.Security;
using RealEstateAdmin.Core.Security.Util;
using RealEstateAdmin.Core.ServiceContracts;
using static RealEstateAdmin.Core.Security.TokenGenerator;

namespace RealEstateAdmin.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenGenerator _tokenGenerator;
        private readonly ILogger<AdminService> _logger;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public AdminService(IUnitOfWork unitOfWork, TokenGenerator tokenGenerator, ILogger<AdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Admin Login
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<ServiceResult> Login(LoginForm form)
        {
            ServiceResult result = new();
            try
            {
                Admin? admin = await _unitOfWork.AdminRepository.FindByEmail(form.Email);

                if (admin is null || !BCrypt.Net.BCrypt.Verify(form.Password, admin.Password))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Credentials";

                    _logger.LogInformation("Invalid Login Email : '{Email}'", form.Email);

                    return result;
                }
                Token accessToken = _tokenGenerator.GenerateAccessToken(admin);

                Token refreshToken = _tokenGenerator.GenerateRefreshToken(admin);

                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Success";
                result.Data = new LoginView(admin, accessToken, refreshToken);

                _logger.LogInformation("Login Success Email : '{Email}'", form.Email);


                return result;

            }
            catch (Exception ex)
            {

                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "AS-01 : Server Error";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            await Task.Delay(10);
            return result;
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ServiceResult> RefreshAsync(string token)
        {
            _logger.LogInformation("Refresh Login Token : '{Token}'", token);
            Admin admin;
            ServiceResult result = new();
            Token refreshToken;
            try
            {
                /// Get Jwt Token And User Id From Base64 Encoded Refresh Token
                var data = GetAdminIdAndTokenData(token);
                var refreshTokenData = data[1];
                /// Get User Object Form Refresh Token Data Which is a jwt Token
                admin = await _unitOfWork.AdminRepository.FindByAdminId(Convert.ToInt32(data[0])) ?? throw new Exception("Invalid UserId");
                /// Get Token Object from Jwt Token For Returning the Token 
                refreshToken = _tokenGenerator.VerifyRefreshToken(refreshTokenData, admin);
            }
            catch (SecurityTokenExpiredException ex)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "Token Expired";

                _logger.LogWarning(ex, "Token Expired");

                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "Invalid Token";

                _logger.LogWarning(ex, "Invalid Token");

                return result;
            }
            /// Generate New AccessToken
            var accessToken = _tokenGenerator.GenerateAccessToken(admin);
            /// Same RefreshToken and New AccessToken is Returened
            result.ServiceStatus = ServiceStatus.Success;
            result.Message = "Success";
            result.Data = new LoginView(admin, accessToken, refreshToken);
            _logger.LogInformation("Refresh Success AccessToken : '{AccessToken}' RefreshToken : '{RefreshToken}'", accessToken.Value, refreshToken.Value);

            return result;
        }
    }
}
