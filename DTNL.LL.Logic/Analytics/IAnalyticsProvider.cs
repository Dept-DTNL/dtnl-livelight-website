using System.Threading.Tasks;
using DTNL.LL.Models;

namespace DTNL.LL.Logic.Analytics
{
    public interface IAnalyticsProvider
    {
        public Task<AnalyticsReport> GetAnalytics(Project project);
    }
}