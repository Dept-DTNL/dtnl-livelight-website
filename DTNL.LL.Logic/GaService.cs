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

        public Task<AnalyticsReport> GetAnalyticsReport(Project project)
        {
            return project.AnalyticsVersion switch
            {
                AnalyticsVersion.V3 => _v3Analytics.GetAnalytics(project),
                AnalyticsVersion.V4 => _v4Analytics.GetAnalytics(project),
                _ => throw new NotImplementedException(),
            };
        }
    }
}