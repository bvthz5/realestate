using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Domain.RepositoryContracts;
using RealEstateUser.Core.DTO.Forms;
using RealEstateUser.Core.DTO.Views;
using RealEstateUser.Core.Enums;
using RealEstateUser.Core.Helpers;
using RealEstateUser.Core.Security;
using RealEstateUser.Core.Security.Util;
using RealEstateUser.Core.ServiceContracts;
using static RealEstateUser.Core.Security.TokenGenerator;

namespace RealEstateUser.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly TokenGenerator _tokenGenerator;

        private readonly IEmailService _emailService;

        private readonly ILogger<UserService> _logger;

        private readonly ImageFileUtil _imageFileUtil;

        private const string ErrorMessageTemplate = "Error occurred: {ErrorMessage}";

        public UserService(IUnitOfWork unitOfWork, TokenGenerator tokenGenerator, IEmailService emailService, ILogger<UserService> logger, ImageFileUtil imageFileUtil)
        {
            this._unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
            _logger = logger;
            _imageFileUtil = imageFileUtil;
        }

        public async Task<ServiceResult> Login(LoginForm form)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.Email == form.Email)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is  {(UserStatus)user.Status}";
                    return result;
                }
                // For Google User Password is Empty
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Password Not Set";
                    return result;
                }

                if (!BCrypt.Net.BCrypt.Verify(form.Password, user.Password))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Invalid Credentials";
                    _logger.LogInformation("Invalid Login Email : '{Email}'", form.Email);
                    return result;
                }

                if (user != null)
                {
                    var accessToken = _tokenGenerator.GenerateAccessToken(user);
                    var refreshToken = _tokenGenerator.GenerateRefreshToken(user);
                    result.Data = new LoginView(user, accessToken, refreshToken);
                    result.ServiceStatus = ServiceStatus.Success;
                    _logger.LogInformation("Login Success Email : '{Email}'", form.Email);
                }
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
        /// When the accesstoken is expired, Refresh token is used to create a new accesstoken for the user.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Loginview with new accessToken and old refreshToken</returns>

        public async Task<ServiceResult> Refresh(string token)
        {
            _logger.LogInformation("Refresh Login Token : '{Token}'", token);
            User user;

            Token refreshToken;

            ServiceResult result = new();
            try
            {
                // Get Jwt Token And User Id From Base64 Encoded Refresh Token
                var data = GetUserIdAndTokenData(token);

                var refreshTokenData = data[1];

                // Get User Object Form Refresh Token Data Which is a jwt Token        
                user = (await _unitOfWork.UserRepository.Find(user => user.UserId == Convert.ToInt32(data[0])
                && user.Status == (int)UserStatus.Active)).SingleOrDefault() ?? throw new Exception("Invalid UserId");

                // Get Token Object from Jwt Token For Returning the Token 
                refreshToken = _tokenGenerator.VerifyRefreshToken(refreshTokenData, user);

                // Generate New AccessToken
                var accessToken = _tokenGenerator.GenerateAccessToken(user);

                // Same RefreshToken and New AccessToken is Returened
                result.Data = new LoginView(user, accessToken, refreshToken);
                result.ServiceStatus = ServiceStatus.Success;
                _logger.LogInformation("Refresh Success AccessToken : '{AccessToken}' RefreshToken : '{RefreshToken}'", accessToken.Value, refreshToken.Value);

            }
            catch (SecurityTokenExpiredException e)
            {
                result.ServiceStatus = ServiceStatus.TokenExpired;
                result.Message = "US-02 : Token Expired";
                _logger.LogError(e, ErrorMessageTemplate, e.Expires);
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.NoRecordFound;
                result.Message = $"User is Invalid";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }

            return result;
        }

        /// <summary>
        /// User registration to the system.After successful registration User gets an email in the given Email.
        /// </summary>
        /// <param name="form"></param>
        /// <returns>UserView</returns>

        public async Task<ServiceResult> AddUser(UserRegistrationForm form)
        {
            ServiceResult result = new();
            try
            {
                string token;
                Guid verificationCode;

                // Check If User Already Registered
                User? user = (await _unitOfWork.UserRepository.Find(user => user.Email == form.Email)).SingleOrDefault();


                if (user != null)
                {
                    if (user.Status == (byte)UserStatus.Active)
                    {
                        result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                        result.Message = "User Already Exists";
                        return result;
                    }
                    if (user.Status == (byte)UserStatus.Inactive)
                    {
                        result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                        result.Message = "User is not Active";
                        return result;
                    }
                    if (user.Status == (byte)UserStatus.Blocked)
                    {
                        result.ServiceStatus = ServiceStatus.RecordAlreadyExists;
                        result.Message = "User is Blocked";
                        return result;
                    }
                    if (user.Status == (byte)UserStatus.Deleted)
                    {
                        // Guid Id Saved in the database for Email Verification Purpose
                        verificationCode = Guid.NewGuid();

                        user.Password = BCrypt.Net.BCrypt.HashPassword(form.Password);
                        user.PhoneNumber = string.Empty;
                        user.FirstName = string.Empty;
                        user.LastName = string.Empty;
                        user.Address = string.Empty;
                        user.CreatedOn = DateTime.Now;
                        user.UpdatedOn = DateTime.Now;

                        user.Status = (byte)UserStatus.Inactive;
                        user.VerificationCode = verificationCode.ToString();
                        user.UpdatedOn = DateTime.Now;

                        await _unitOfWork.UserRepository.Update(user);

                        await _unitOfWork.SaveAsync();

                        // Generating Email Verification Token
                        token = Convert.ToBase64String(Encoding.Unicode.GetBytes($"{user.UserId}#{user.Email}#{verificationCode}"));

                        _emailService.VerifyUser(form.Email, token);

                        result.Data = new UserView(user);
                        result.ServiceStatus = ServiceStatus.Success;
                        result.Message = "Your registration is complete! We have successfully sent a verification code to your email address.";

                        return result;
                    }
                }

                // Guid Id Saved in the database for Email Verification Purpose
                verificationCode = Guid.NewGuid();

                user = await _unitOfWork.UserRepository.Add(new()
                {
                    Email = form.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(form.Password),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Status = (byte)UserStatus.Inactive,
                    VerificationCode = verificationCode.ToString(),
                });

                await _unitOfWork.SaveAsync();

                // Generating Email Verification Token
                token = Convert.ToBase64String(Encoding.Unicode.GetBytes($"{user.UserId}#{user.Email}#{verificationCode}"));

                _emailService.VerifyUser(form.Email, token);

                result.Data = new UserView(user);
                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Your registration is complete! We have successfully sent a verification code to your email address.";
                return result;

            }
            catch (Exception e)
            {

                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-04: Server Error";
                Console.WriteLine(e.Message);
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }

        /// <summary>
        /// Verfication email is sent again to the user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>string</returns>

        public async Task<ServiceResult> ResendVerificationMail(string email)
        {
            // Initialize a new ServiceResult object to hold the result of the method.
            ServiceResult result = new();
            try
            {
                // Initialize a nullable User variable called user
                // Call the Find method of the _unitOfWork.UserRepository object
                // and pass in a lambda expression to filter the results
                // Call the SingleOrDefault method on the resulting collection
                // to return a single User object or null if there are no results
                User? user = (await _unitOfWork.UserRepository.Find(user => user.Email == email)).SingleOrDefault();

                // If the user is not found, return a not found error.
                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }

                // If the user's status is not "Inactive", return a bad request error.
                if (user.Status != (byte)UserStatus.Inactive)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }
                // Guid Id Saved in the database for Email Verification Purpose
                Guid verificationCode = Guid.NewGuid();

                // Generating Email Verification Token by encoding the user ID, email address, and verification code in a Base64 string.
                string token = Convert.ToBase64String(Encoding.Unicode.GetBytes($"{user.UserId}#{user.Email}#{verificationCode}"));

                // Use the email service to send the verification email to the user.
                _emailService.VerifyUser(user.Email, token);

                // Update the user's verification code and updated date in the user repository.
                user.VerificationCode = verificationCode.ToString();
                user.UpdatedOn = DateTime.Now;
                await _unitOfWork.UserRepository.Update(user);

                // Save the changes to the database.
                await _unitOfWork.SaveAsync();

                result.Data = new UserView(user);
                result.ServiceStatus = ServiceStatus.Success;

                // Set the message property of the ServiceResult object to indicate that the email was sent successfully.
                result.Message = "Email Sent Successfully";

                // Return the ServiceResult object.
                return result;

            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;

                // Set the Message property of the result object to a specific error message, including a code
                result.Message = "US-05: ServerError";

                // Write the exception message to the console
                Console.WriteLine(e.Message);

                // Log the exception with a custom error message template using a logger object
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }

        /// <summary>
        /// Verification tocken from email is sent back to verify the user and cahnge the user status to active state. After verification user can lof=gin to the system.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>String</returns>

        public async Task<ServiceResult> VerifyUser(string token)
        {
            int userId;
            string email = "";
            string verificationCode = "";
            ServiceResult result = new();

            // Get Data from Email Verification Token
            try
            {
                var data = Encoding.Unicode.GetString(Convert.FromBase64String(token)).Split("#");

                userId = Convert.ToInt32(data[0]);

                email = data[1];

                verificationCode = data[2];
            }

            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "US-06: Invalid Token";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
                return result;
            }
            try
            {
                // Check if Token id Valid
                var user = (await _unitOfWork.UserRepository.Find(user => user.Email == email && user.VerificationCode == verificationCode)).FirstOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User Not Found";
                    return result;
                }

                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $" User is {(UserStatus)(sbyte)user.Status}";
                    return result;
                }
                if (DateTime.Now - user.UpdatedOn > TimeSpan.FromMinutes(10))
                {
                    // If the verification token has expired, return a bad request error.
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Token Expired";
                    return result;
                }

                user.VerificationCode = null;
                user.Status = (byte)UserStatus.Active;
                user.UpdatedOn = DateTime.Now;

                await _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveAsync();

                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "User Verified";
            }
            catch (Exception ex)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = $"US-07 ServerError: {ex.Message}";
                _logger.LogError(ex, ErrorMessageTemplate, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        public async Task<ServiceResult> ForgotPasswordRequest(string email)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.Email == email && user.Status == (byte)UserStatus.Active)).SingleOrDefault();
                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }

                // Guid is Userd For Forgot Password Token Generation and is Stored In Database
                string verificationCode = $"{Guid.NewGuid()}${DateTime.Now}";

                user.VerificationCode = verificationCode;

                await _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveAsync();

                // Forgot Password Token Generation
                string token = Convert.ToBase64String(Encoding.Unicode.GetBytes($"{user.UserId}#{user.Email}#{verificationCode}"));

                _emailService.ForgotPassword(user.Email, token);

                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Request Send Succesfully";
                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-08: ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }

        /// <summary>
        /// Reset password is to reset a passwoord forgot by user.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>

        public async Task<ServiceResult> ResetPassword(ForgotPasswordForm form)
        {
            int userId;
            string email = "";
            string verificationCode = "";
            DateTime tokenGeneratedTime = new();
            ServiceResult result = new();

            // Getting Data From Forgot Password Token
            try
            {
                var data = Encoding.Unicode.GetString(Convert.FromBase64String(form.Token)).Split('#');

                userId = Convert.ToInt32(data[0]);

                email = data[1];

                verificationCode = data[2];

                tokenGeneratedTime = DateTime.Parse(verificationCode.Split('$')[1]);
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.InvalidRequest;
                result.Message = "US-09 : Invalid Token";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            try
            {
                // Check for Token Status   
                var user = (await _unitOfWork.UserRepository.Find(user => user.Email == email && user.VerificationCode == verificationCode)).FirstOrDefault();

                if (user == null || DateTime.Now - tokenGeneratedTime > TimeSpan.FromMinutes(15))
                {
                    result.ServiceStatus = ServiceStatus.TokenExpired;
                    result.Message = "Token Expired";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"user is {(UserStatus)user.Status}";
                    return result;
                }


                user.Password = BCrypt.Net.BCrypt.HashPassword(form.Password);
                user.VerificationCode = null;
                user.UpdatedOn = DateTime.Now;

                await _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveAsync();

                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "User Password Changed";
            }
            catch (Exception x)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = $"US-10 ServerError:{x.Message}";
                _logger.LogError(x, ErrorMessageTemplate, x.Message);
            }
            return result;

        }

        public async Task<ServiceResult> UploadImage(int userId, ImageForm image)
        {
            ServiceResult result = new();

            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User Not Found";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Inactive) || user.Status == ((byte)UserStatus.Blocked))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }
                if (image.File.Length > 0)
                {
                    // Delete Already Existing Picture If present
                    if (user.ProfilePic != null)
                    {
                        _imageFileUtil.DeleteUserProfiePic(user.ProfilePic);
                    }
                    // Upload Image To as a file To a specific Location
                    var file = _imageFileUtil.UploadUserProfiePic(user, image.File);

                    if (file == null)
                    {
                        result.ServiceStatus = ServiceStatus.InvalidRequest;
                        result.Message = "Failed To Upload";
                        return result;
                    }

                    user.ProfilePic = file;
                    await _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.SaveAsync();
                    result.ServiceStatus = ServiceStatus.Success;
                    result.Message = $"Success : Image Uploaded" + $" , FileName : {file}";
                    return result;
                }
                else
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Failed";
                    return result;
                }
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-11: ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }

        public async Task<FileStream?> GetProfile(string fileName)
        {
            // Check if the profile picture exists in the file system.
            if (!await _unitOfWork.UserRepository.IsProfilePicExists(fileName))
                return null;

            // If the profile picture exists, retrieve it using the file utility.
            return _imageFileUtil.GetUserProfilePic(fileName);
        }

        /// <summary>
        /// Method to change password. To change current password and set a new password
        /// </summary>
        /// <param name="form"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<ServiceResult> ChangePassword(ChangePasswordForm form, int userId)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = "User Not Found";
                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Inactive) || user.Status == ((byte)UserStatus.Blocked))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }

                // For Google User Password is Empty and Checking that Case
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Password Not Set";
                    return result;
                }
                if (!BCrypt.Net.BCrypt.Verify(form.CurrentPassword, user.Password))
                {
                    result.ServiceStatus = ServiceStatus.InvalidRequest;
                    result.Message = "Password MissMatch";
                    return result;
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(form.NewPassword);

                await _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveAsync();

                result.ServiceStatus = ServiceStatus.Success;
                result.Message = "Password changed";
                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-12: ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }

        /// <summary>
        /// Method to veiw a users detail.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Userdetailview</returns>
        public async Task<ServiceResult> GetUserAsync(int userId)
        {
            ServiceResult result = new();
            try
            {
                var user = (await _unitOfWork.UserRepository.Find(user => user.UserId == userId)).SingleOrDefault();

                if (user == null)
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User Not Found";

                    return result;
                }
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }

                bool hasPassword = !string.IsNullOrEmpty(user.Password);

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = new UserDetailView(user, hasPassword);

                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-13:ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }


        /// <summary>
        /// Method where user can update his profile.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<ServiceResult> EditAsync(int userId, UserUpdateForm form)
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
                if (user.Status == ((byte)UserStatus.Deleted) || user.Status == ((byte)UserStatus.Blocked) || user.Status == ((byte)UserStatus.Inactive))
                {
                    result.ServiceStatus = ServiceStatus.NoRecordFound;
                    result.Message = $"User is {(UserStatus)user.Status}";
                    return result;
                }


                user.FirstName = form.FirstName.Trim();
                user.LastName = form.LastName?.Trim();
                user.Address = form.Address?.Trim();
                user.PhoneNumber = form.PhoneNumber;


                await _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveAsync();

                result.ServiceStatus = ServiceStatus.Success;
                result.Data = new UserDetailView(user);
                return result;
            }
            catch (Exception e)
            {
                result.ServiceStatus = ServiceStatus.ServerError;
                result.Message = "US-14:ServerError";
                _logger.LogError(e, ErrorMessageTemplate, e.Message);
            }
            return result;
        }
    }
}
