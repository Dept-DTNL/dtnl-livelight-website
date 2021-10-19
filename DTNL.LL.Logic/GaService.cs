using System;
using System.Threading.Tasks;
using DTNL.LL.Logic.Analytics;
using DTNL.LL.Models;

namespace DTNL.LL.Logic
{
    public class GaService
    {
        private readonly V3Analytics _v3Analytics;
        private readonly V4Analytics _v4Analytics;


        public GaService(V3Analytics v3Analytics, V4Analytics v4Analytics)
        {
            _v3Analytics = v3Analytics;
            _v4Analytics = v4Analytics;
        }

        public async Task<AnalyticsReport> GetAnalyticsReport(Project project, int timeSinceLastTick)
        {
            switch(project.AnalyticsVersion)
            {
                case AnalyticsVersion.V3:
                    return await _v3Analytics.GetAnalytics(project, timeSinceLastTick);
                case AnalyticsVersion.V4:
                    return await _v4Analytics.GetAnalytics(project, timeSinceLastTick);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}