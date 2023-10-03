using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.DTO.Views;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security;
using RealEstateUser.Core.ServiceContracts;

namespace RealEstateUser.Core.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly GoogleSecurity _googleSecurity;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenGenerator _tokenGenerator;
        private readonly ILogger<GoogleService> _logger;
        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public GoogleService(GoogleSecurity googleAuth, IUnitOfWork unitOfWork, TokenGenerator tokenGenerator, ILogger<GoogleService> logger)
        {
            _unitOfWork = unitOfWork;
            _googleSecurity = googleAuth;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user or logs in an existing user with Google authentication.
        /// </summary>
        /// <param name="idToken">The Google ID token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<ServiceResult> Login(string idToken)
        {
            // Create a new service result object
            ServiceResult result = new();
            try
            {
                // Verify the Google ID token
                var info = await _googleSecurity.VerifyGoogleToken(idToken);

                // If the token is invalid, return a bad request response
                if (info == null)
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Credentials";

                    return result;
                }


                User? user = (await _unitOfWork.UserRepository.Find(user => user.Email == info.Email)).SingleOrDefault();

                if (user != null && user.Status == (byte)UserStatus.Deleted)
                {
                    user.FirstName = info.GivenName;
                    user.LastName = info.FamilyName;
                    user.Email = info.Email;
                    user.CreatedOn = DateTime.Now;
                    user.UpdatedOn = DateTime.Now;
                    user.Status = (byte)UserStatus.Active;

                    user.UpdatedOn = DateTime.Now;

                    await _unitOfWork.UserRepository.Update(user);

                    await _unitOfWork.SaveAsync();

                    result.ServiceStatus = ServiceStatus.Success;
                    result.Data = new LoginView(user, _tokenGenerator.GenerateAccessToken(user), _tokenGenerator.GenerateRefreshToken(user));
                    result.Message = "User Logged In";
                    return result;

                }

                // If the user not exists
                if (user == null)
                {
                    user = await _unitOfWork.UserRepository.Add(new User()
                    {
                        FirstName = info.GivenName,
                        LastName = info.FamilyName,
                        Email = info.Email,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        Status = (byte)UserStatus.Active
                    });

                    await _unitOfWork.SaveAsync();
                }

                else if (user.Status == (byte)UserStatus.Inactive)
                {
                    user.Status = (byte)UserStatus.Active;
                    user.UpdatedOn = DateTime.Now;

                    await _unitOfWork.UserRepository.Update(user);

                    await _unitOfWork.SaveAsync();
                }

                if (user.Status == (byte)UserStatus.Blocked)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }
                result.ServiceStatus = ServiceStatus.Success;
                result.Data = new LoginView(user, _tokenGenerator.GenerateAccessToken(user), _tokenGenerator.GenerateRefreshToken(user));
                result.Message = "User Logged In";
                return result;
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "GS=01: ServerError";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;


        }
    }
}
