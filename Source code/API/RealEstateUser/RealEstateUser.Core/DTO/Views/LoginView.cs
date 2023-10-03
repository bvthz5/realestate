using RealEstate.Domain.Data.Entities;
using static RealEstateUser.Core.Security.TokenGenerator;

namespace RealEstateUser.Core.DTO.Views
{
    public class LoginView : UserView
    {
        public class TokenView
        {
            public string? Value { get; }

            public DateTime Expiry { get; }

            public TokenView(Token token)
            {
                Value = token.Value;
                Expiry = token.Expiry;
            }
        }
        public TokenView AccessToken { get; }

        public TokenView RefreshToken { get; }
        public new string FirstName { get; set; }
        public new string? LastName { get; set; }
        public string? Address { get; set; }

        public LoginView(User user, Token AccessToken, Token RefreshToken) : base(user)
        {
            this.AccessToken = new TokenView(AccessToken);

            this.RefreshToken = new TokenView(RefreshToken);

            FirstName = user.FirstName;

            LastName = user.LastName;

            Address = user.Address;
        }

    }
}
