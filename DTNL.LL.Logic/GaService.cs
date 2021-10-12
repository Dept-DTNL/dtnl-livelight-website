using System;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.Extensions.Configuration;

namespace DTNL.LL.Logic
{
    public class GaService
    {
        private readonly string _gaProperties;
        private readonly string _gaActiveUsers;
        private readonly BetaAnalyticsDataClient _gaClient;

        public GaService(IConfiguration config, GoogleCredentialProviderService googleCredentialProvider)
        {
        }

        public async Task<AnalyticsReport> GetAnalyticsTrafficReport(string propertyId, int timeRangeInMinutes, int projectId)
        {
            throw new NotImplementedException();
        }
    }
}