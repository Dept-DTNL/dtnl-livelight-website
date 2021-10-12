using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google.Analytics.Data.V1Beta;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;

namespace DTNL.LL.Logic.Analytics
{
    public class V4Analytics : IAnalyticsProvider
    {

        private readonly string _gaProperties;
        private readonly string _gaActiveUsers;
        private readonly BetaAnalyticsDataClient _gaClient;

        public V4Analytics(IConfiguration config, GoogleCredentialProviderService googleCredentialProvider)
        {
            var apiTags = new GaApiTagsOptions();
            config.GetSection(GaApiTagsOptions.GaApiTags).Bind(apiTags);
            _gaProperties = apiTags.GaProperties;
            _gaActiveUsers = apiTags.GaActiveUsers;
            var credentials = googleCredentialProvider.GetGoogleCredentials();
            var builder = new BetaAnalyticsDataClientBuilder
            {
                ChannelCredentials = credentials.ToChannelCredentials()
            };
            _gaClient = builder.Build();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyId">The analytics property Id</param>
        /// <param name="timeRangeInMinutes">The time in minutes how long the request should look back for active users</param>
        /// <returns></returns>
        private RunRealtimeReportRequest CreateRealtimeReportRequest(string propertyId, int timeRangeInMinutes)
        {
            return new RunRealtimeReportRequest()
            {
                Property = _gaProperties + propertyId,
                Metrics = { new Metric { Name = _gaActiveUsers }, },
                MinuteRanges = { new MinuteRange { StartMinutesAgo = timeRangeInMinutes, EndMinutesAgo = 0 } }
            };
        }


        public async Task<AnalyticsReport> GetAnalytics(string property, int timeRangeInMinutes)
        {
            var request = CreateRealtimeReportRequest(property, timeRangeInMinutes);
            var response = await _gaClient.RunRealtimeReportAsync(request);
            var activeUsers = response.Rows.ElementAtOrDefault(0)?.MetricValues[0].Value ?? "0";

            return new AnalyticsReport()
            {
                ActiveUsers = int.Parse(activeUsers),
            };
        }
    }
}