using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Google;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Services;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic.Analytics
{
    public class V3Analytics : IAnalyticsProvider
    {
        private const int ResponseMinutesAgoArrayPos = 0;
        private const int ConvertsArrayPos = 1;

        private readonly AnalyticsService _analyticsService;
        private readonly GaApiTagsOptions _options;
        public V3Analytics(IOptions<GaApiTagsOptions> config, GoogleCredentialProviderService credentialProvider)
        {
            _options = config.Value;
            _analyticsService = new AnalyticsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentialProvider.GetGoogleCredentials()
            });
        }

        public async Task<AnalyticsReport> GetAnalytics(Project project)
        {
            DataResource.RealtimeResource.GetRequest activeUsersRequest = _analyticsService.Data.Realtime.Get(project.GaProperty, _options.Ga3ActiveUsers);
            List<DataResource.RealtimeResource.GetRequest> conversionRequests = GetConversionRequests(project);

            Task<RealtimeData> activeUserResponseTask = activeUsersRequest.ExecuteAsync();
            List<Task<RealtimeData>> conversionTasks = new List<Task<RealtimeData>>(conversionRequests.Count);
            conversionRequests.ForEach(r => conversionTasks.Add(r.ExecuteAsync()));

            // Wait until both tasks are finished so we can process them.
            RealtimeData[] conversionResponses = await Task.WhenAll(conversionTasks);
            await activeUserResponseTask;

            return new AnalyticsReport
            {
                Project = project,
                ActiveUsers = GetActiveUserCount(activeUserResponseTask.Result),
                Conversions = GetAmountOfConversions(conversionResponses, project.PollingTimeInMinutes)
            };
        }

        private List<DataResource.RealtimeResource.GetRequest> GetConversionRequests(Project project)
        {
            List<DataResource.RealtimeResource.GetRequest> conversionRequests = new List<DataResource.RealtimeResource.GetRequest>();

            if (project.ConversionTags is null)
                return conversionRequests;

            foreach (string conversionTag in project.ConversionTags)
            {
                DataResource.RealtimeResource.GetRequest conversionRequest = _analyticsService.Data.Realtime.Get(project.GaProperty, conversionTag);
                conversionRequest.Dimensions = _options.Ga3MinutesAgo;
                conversionRequests.Add(conversionRequest);
            }

            return conversionRequests;
        }

        private int GetActiveUserCount(RealtimeData data)
        {
            return int.Parse(data.Rows?.ElementAtOrDefault(0)?[0] ?? "0");
        }

        private int GetAmountOfConversions(RealtimeData[] responses, int pollingTimeInMinutes)
        {
            int converts = 0;

            if (responses.Length == 0)
                return converts;

            foreach (RealtimeData response in responses)
                converts += ParseConvertRows(response.Rows, pollingTimeInMinutes);

            return converts;
        }

        private int ParseConvertRows(IList<IList<string>> rows, int pollingTimeInMinutes)
        {
            if (rows is null)
                return 0;

            int responseTotalConverts = 0;

            foreach (IList<string> row in rows)
            {
                //Data structure in rows: 2d array. 1st dimension: Grouped by minutes ago, second dimension sorted [0] minutes ago, [1] conversions
                int goalTriggeredMinutesAgo = int.Parse(row[ResponseMinutesAgoArrayPos]);
                if (goalTriggeredMinutesAgo >= pollingTimeInMinutes)
                    continue;
                responseTotalConverts += int.Parse(row[ConvertsArrayPos]);
            }

            return responseTotalConverts;
        }
    }
}