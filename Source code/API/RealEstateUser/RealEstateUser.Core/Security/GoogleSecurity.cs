using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using RealEstateUser.Core.Settings;

namespace RealEstateUser.Core.Security
{
    public class GoogleSecurity
    {
        private readonly GoogleSettings _googleSettings;
        public GoogleSecurity(IOptions<GoogleSettings> googleSettings)
        {
            _googleSettings = googleSettings.Value;
        }

        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _googleSettings.ClientId }
                };

                return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return null;
            }
        }
    }
}
