using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Settings;

namespace RealEstateUser.Core.Security
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
        public Token GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.UserId.ToString()),
            };
            var expiry = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiry);

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
               _jwtSettings.Audience,
                claims,
                expires: expiry,
                signingCredentials: credentials);

            return new Token(new JwtSecurityTokenHandler().WriteToken(token), expiry);
        }
        public Token GenerateRefreshToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_jwtSettings.Key}{user.Password}"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email)
            };

            var expiry = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpiry);

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
               _jwtSettings.Audience,
                claims: claims,
                expires: expiry.ToUniversalTime(),
                signingCredentials: credentials);

            string refreshToken = Convert.ToBase64String(Encoding.Unicode.GetBytes($"{user.UserId}{_seperator}{new JwtSecurityTokenHandler().WriteToken(token)}"));

            return new Token(refreshToken, expiry);
        }

        /// <summary>
        /// Verify the Integrity of RefreshToken (Jwt Token) and Return Token Object
        /// </summary>
        /// <param name="tokenData">Refresh Token : Jwt Token</param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public Token VerifyRefreshToken(string tokenData, User user)
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_jwtSettings.Key}{user.Password}"))
            };

            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(tokenData, tokenValidationParameters, out SecurityToken securityToken);

            var email = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("Email not Present");

            if (email.Value != user.Email)
                throw new SecurityTokenException("Invalid Token");

            return new Token(Convert.ToBase64String(Encoding.Unicode.GetBytes($"{user.UserId}{_seperator}{tokenData}")), securityToken.ValidTo.ToLocalTime());
        }

        /// <summary>
        /// Takes Base64Encoded Refresh Token <br/>
        /// Parse it and returns userId and Jwt Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>string[0] userId <br/>
        /// string[1] jwt token</returns>
        public static string[] GetUserIdAndTokenData(string refreshToken)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(refreshToken)).Split(_seperator);
        }
    }
}
