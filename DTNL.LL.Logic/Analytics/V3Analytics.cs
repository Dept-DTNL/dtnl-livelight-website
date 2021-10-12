using System;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google.Apis.Analytics.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace DTNL.LL.Logic.Analytics
{
    public class V3Analytics : IAnalyticsProvider
    {
        private readonly AnalyticsService _analyticsService;

        private readonly string ActiveUsersTag;

        public V3Analytics(IConfiguration config, GoogleCredentialProviderService credentialProvider)
        {
            var tagOptions = new GaApiTagsOptions();
            config.GetSection(GaApiTagsOptions.GaApiTags).Bind(tagOptions);
            ActiveUsersTag = tagOptions.Ga3ActiveUsers;
            _analyticsService = new AnalyticsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentialProvider.GetGoogleCredentials()
            });
        }
        
        public async Task<AnalyticsReport> GetAnalytics(string property, int timeRangeInMinutes)
        {
            var request = _analyticsService.Data.Realtime.Get(property, ActiveUsersTag);
            var response = await request.ExecuteAsync();
            var activeUsers = response.Rows.ElementAtOrDefault(0)?[0] ?? "0";
            
            return new AnalyticsReport()
            {
                ActiveUsers = int.Parse(activeUsers)
            };
        }
    }
}