using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic.Analytics
{
    public class V4Analytics : IAnalyticsProvider
    {
        private readonly ILogger<V4Analytics> _logger;
        private readonly string _gaProperties;
        private readonly string _gaActiveUsers;
        private readonly string _gaEventName;
        private readonly string _gaConversions;
        private readonly BetaAnalyticsDataClient _gaClient;

        public V4Analytics(ILogger<V4Analytics> logger, IOptions<GaApiTagsOptions> config, GoogleCredentialProviderService googleCredentialProvider)
        {
            _logger = logger;
            GaApiTagsOptions apiTags = config.Value;
            _gaProperties = apiTags.Ga4Properties;
            _gaActiveUsers = apiTags.Ga4ActiveUsers;
            _gaEventName = apiTags.Ga4EventName;
            _gaConversions = apiTags.Ga4Conversions;
            GoogleCredential credentials = googleCredentialProvider.GetGoogleCredentials();
            BetaAnalyticsDataClientBuilder builder = new()
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
            RunRealtimeReportRequest request = new()
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
            Task<RunRealtimeReportResponse> activeUsersResponse =
                _gaClient.RunRealtimeReportAsync(CreateActiveUsersRequest(project.GaProperty,
                    project.PollingTimeInMinutes));
            Task<RunRealtimeReportResponse> conversionsResponse =
                _gaClient.RunRealtimeReportAsync(CreateConversionsRequest(project.GaProperty,
                    project.PollingTimeInMinutes));
            try
            {
                await Task.WhenAll(activeUsersResponse, conversionsResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Not authorized to access the GA property");
                return new AnalyticsReport()
                {
                    Project = project,
                    ActiveUsers = GetActiveUsers(new RunRealtimeReportResponse()),
                    Conversions = GetConversions(new RunRealtimeReportResponse(), project.ConversionTags)
                };
            }

            return new AnalyticsReport()
            {
                Project = project,
                ActiveUsers = GetActiveUsers(activeUsersResponse.Result ?? new RunRealtimeReportResponse()),
                Conversions = GetConversions(conversionsResponse.Result ?? new RunRealtimeReportResponse(), project.ConversionTags)
            };
        }

        private static int GetActiveUsers(RunRealtimeReportResponse response) => int.Parse(response.Rows.ElementAtOrDefault(0)?.MetricValues[0].Value ?? "0");

        private static int GetConversions(RunRealtimeReportResponse response, List<string> conversionTags)
        {
            int conversions = 0;
            if (response.RowCount is 0)
                return 0;
            foreach (Row row in response.Rows)
            {
                string dimensionValue = row.DimensionValues[0].Value;
                if(!conversionTags.Contains(dimensionValue))
                    continue;
                int metricsAmount = int.Parse(row.MetricValues[0].Value);
                conversions += metricsAmount;
            }
            return conversions;
        }
    }
}