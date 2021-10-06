using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google.Analytics.Data.V1Beta;
using Microsoft.Extensions.Configuration;

namespace DTNL.LL.Logic
{
    public class GaService
    {
        private readonly string _gaProperties;
        private readonly string _gaMinutesAgo;
        private readonly string _gaActiveUsers;
        private readonly BetaAnalyticsDataClient _gaClient;

        public GaService(IConfiguration config)
        {
            var apiTags = new GaApiTagsOptions();
            config.GetSection(GaApiTagsOptions.GaApiTags).Bind(apiTags);
            _gaProperties = apiTags.GaProperties;
            _gaActiveUsers = apiTags.GaActiveUsers;
            _gaClient = BetaAnalyticsDataClient.Create();
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
                MinuteRanges = { new MinuteRange { StartMinutesAgo = 0, EndMinutesAgo = timeRangeInMinutes } }
            };
        }

        public async Task<AnalyticsReport> GetAnalyticsTrafficReport(string propertyId, int timeRangeInMinutes)
        {
            var request = CreateRealtimeReportRequest(propertyId, timeRangeInMinutes);
            var response = await _gaClient.RunRealtimeReportAsync(request);
            var activeUsers = response.Rows.ElementAtOrDefault(0)?.MetricValues[0].Value ?? "0";

           return new AnalyticsReport()
            {
                ActiveUsers = int.Parse(activeUsers)
            };

        }
    }
}