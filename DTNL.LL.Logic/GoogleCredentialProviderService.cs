using DTNL.LL.Logic.Options;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic
{
    public class GoogleCredentialProviderService
    {
        private readonly GAuthOptions _gAuthOptions;

        public GoogleCredentialProviderService(IOptions<GAuthOptions> config)
        {
            _gAuthOptions = config.Value;
        }

        public GoogleCredential GetGoogleCredentials()
        {
            return GoogleCredential.FromJson(_gAuthOptions.Analytics);
        }
    }
}
