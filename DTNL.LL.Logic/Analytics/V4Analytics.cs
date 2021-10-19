using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google.Analytics.Data.V1Beta;
using Google.Protobuf.Collections;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic.Analytics
{
    public class V4Analytics : IAnalyticsProvider
    {

        private readonly string _gaProperties;
        private readonly string _gaActiveUsers;
        private readonly string _gaEventName;
        private readonly string _gaConversions;
        private readonly BetaAnalyticsDataClient _gaClient;

        public V4Analytics(IOptions<GaApiTagsOptions> config, GoogleCredentialProviderService googleCredentialProvider)
        {
            var apiTags = config.Value;
            _gaProperties = apiTags.Ga4Properties;
            _gaActiveUsers = apiTags.Ga4ActiveUsers;
            _gaEventName = apiTags.Ga4EventName;
            _gaConversions = apiTags.Ga4Conversions;
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
        /// <param name="pollingTimeInMinutes">The time in minutes how long the request should look back for active users</param>
        /// <param name="metrics">GA Metric: https://developers.google.com/analytics/devguides/reporting/data/v1/api-schema</param>
        /// <param name="dimensions">GA Dimension. Can be null</param>
        /// <returns></returns>
        private RunRealtimeReportRequest CreateRealtimeReportRequest(Metric[] metrics, Dimension[] dimensions, string propertyId, int pollingTimeInMinutes)
        {
            var request = new RunRealtimeReportRequest()
            {
                Property = _gaProperties + propertyId,
                Metrics = { metrics },
                MinuteRanges = { new MinuteRange { StartMinutesAgo = pollingTimeInMinutes, EndMinutesAgo = 0 } }
            };
            if(dimensions is not null)
                request.Dimensions.AddRange(dimensions);
            return request;
        }

        private RunRealtimeReportRequest CreateActiveUsersRequest(string propertyId, int pollingTimeInMinutes)
        {
            return CreateRealtimeReportRequest(new[] {new Metric {Name = _gaActiveUsers}}, null, propertyId,
                pollingTimeInMinutes);
        }

        private RunRealtimeReportRequest CreateConversionsRequest(string propertyId, int pollingTimeInMinutes)
        {
            return CreateRealtimeReportRequest(new[] {new Metric {Name = _gaConversions}},
                new[] {new Dimension {Name = _gaEventName}}, propertyId, pollingTimeInMinutes);
        }

        public async Task<AnalyticsReport> GetAnalytics(Project project)
        {
            var activeUsersResponse = _gaClient.RunRealtimeReportAsync(CreateActiveUsersRequest(project.GaProperty, project.PollingTimeInMinutes));
            var conversionsResponse =
                _gaClient.RunRealtimeReportAsync(CreateConversionsRequest(project.GaProperty, project.PollingTimeInMinutes));

            await Task.WhenAll(activeUsersResponse, conversionsResponse);

            return new AnalyticsReport()
            {
                ProjectId = project.Id,
                ActiveUsers = GetActiveUsers(activeUsersResponse.Result),
                Conversions = GetConversions(conversionsResponse.Result, project.ConversionTags)
            };
        }

        public int GetActiveUsers(RunRealtimeReportResponse response) => int.Parse(response.Rows.ElementAtOrDefault(0)?.MetricValues[0].Value ?? "0");

        public int GetConversions(RunRealtimeReportResponse response, List<string> conversionTags)
        {
            var conversions = 0;
            if (response.RowCount is 0)
                return 0;
            foreach (var row in response.Rows)
            {
                var dimensionValue = row.DimensionValues[0].Value;
                if(!conversionTags.Contains(dimensionValue))
                    continue;
                var metricsAmount = int.Parse(row.MetricValues[0].Value);
                conversions += metricsAmount;
            }
            return conversions;
        }
    }
}