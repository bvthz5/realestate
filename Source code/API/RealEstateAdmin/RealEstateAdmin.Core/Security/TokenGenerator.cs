
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Settings;

namespace RealEstateAdmin.Core.Security
{
    public class TokenGenerator
    {
        public class Token
        {
            public string Value { get; }

            public DateTime Expiry { get; }

            public Token(string token, DateTime expiry)
            {
                Value = token;
                Expiry = expiry;
            }
        }

        private readonly JwtSettings _jwtSettings;

        private const string _seperator = "#.#";

        public TokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        /// <summary>
        /// Generated Access Token For a particaular Admin
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>Token</returns>
        public Token GenerateAccessToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,admin.AdminId.ToString()),
            };

            var expiry = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiry);

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
               _jwtSettings.Audience,
                claims,
                expires: expiry,
                signingCredentials: credentials);

            return new Token(new JwtSecurityTokenHandler().WriteToken(token), expiry);
        }

        /// <summary>
        /// Generated Refresh Token For a particaular Admin <br/>
        /// Token is signed using key and admin password combination
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>Token</returns>
        public Token GenerateRefreshToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_jwtSettings.Key}{admin.Password}"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,admin.Email)
            };

            var expiry = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpiry);

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
               _jwtSettings.Audience,
                claims: claims,
                expires: expiry.ToUniversalTime(),
                signingCredentials: credentials);

            string refreshToken = Convert.ToBase64String(Encoding.Unicode.GetBytes($"{admin.AdminId}{_seperator}{new JwtSecurityTokenHandler().WriteToken(token)}"));

            return new Token(refreshToken, expiry);
        }


        /// <summary>
        /// Verify the Integrity of RefreshToken (Jwt Token) and Return Token Object
        /// </summary>
        /// <param name="tokenData">Refresh Token : Jwt Token</param>
        /// <param name="admin"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="SecurityTokenException"></exception>
        public Token VerifyRefreshToken(string tokenData, Admin admin)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_jwtSettings.Key}{admin.Password}"))
            };

            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(tokenData, tokenValidationParameters, out SecurityToken securityToken);

            var email = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("Email not Present");

            if (email.Value != admin.Email)
                throw new SecurityTokenException("Invalid Token");

            return new Token(Convert.ToBase64String(Encoding.Unicode.GetBytes($"{admin.AdminId}{_seperator}{tokenData}")), securityToken.ValidTo.ToLocalTime());
        }

        /// <summary>
        /// Takes Base64Encoded Refresh Token <br/>
        /// Parse it and returns adminId and Jwt Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>string[0] adminId <br/>
        /// string[1] jwt token</returns>
        public static string[] GetAdminIdAndTokenData(string refreshToken)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(refreshToken)).Split(_seperator);
        }

    }
}
