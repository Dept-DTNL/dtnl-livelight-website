using DTNL.LL.Logic.Options;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace DTNL.LL.Logic
{
    public class GoogleCredentialProviderService
    {
        private readonly IConfiguration _configuration;

        public GoogleCredentialProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GoogleCredential GetGoogleCredentials()
        {
            var gAuth = new GAuthOptions();
            _configuration.GetSection(GAuthOptions.GAuth).Bind(gAuth);
            return GoogleCredential.FromFile(gAuth.Analytics);
        }
    }
}
