using RealEstate.Domain.Data.Entities;
using static RealEstateAdmin.Core.Security.TokenGenerator;

namespace RealEstateAdmin.Core.DTO.Views
{
    public class LoginView : AdminView
    {
        public class TokenView
        {
            public string Value { get; }
            public DateTime Expiry { get; }
            public TokenView(Token token)
            {
                Value = token.Value;
                Expiry = token.Expiry;
            }
        }
        public TokenView AccessToken { get; }
        public TokenView RefreshToken { get; }
        public LoginView(Admin admin, Token accessToken, Token refreshToken) : base(admin)
        {
            AccessToken = new TokenView(accessToken);

            RefreshToken = new TokenView(refreshToken);
        }
    }
}
