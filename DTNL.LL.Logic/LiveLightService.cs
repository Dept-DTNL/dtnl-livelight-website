using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic
{
    public class LiveLightService
    {
        private readonly int _scanDelayTimeInSeconds;

        private readonly ILogger<LiveLightService> _logger;
        private readonly GaService _gaService;
        private readonly ProjectService _projectService;

        public LiveLightService(ILogger<LiveLightService> logger, IOptions<ServiceWorkerOptions> options, GaService gaService, ProjectService projectService)
        {
            _logger = logger;
            _gaService = gaService;
            _projectService = projectService;
            _scanDelayTimeInSeconds = options.Value.TickTimeInSeconds;
        }

        public async Task ProcessLiveLights()
        {
            //Todo: Check if project is not off due to lockout. E.g. 5pm - 9am
            var projects = await _projectService.GetActiveProjects();
            var analyticsReportTasks = projects.Select(
                project => _gaService.GetAnalyticsReport(project, _scanDelayTimeInSeconds));

            var analyticsReports = await Task.WhenAll(analyticsReportTasks.ToArray());
            // Todo Create a LightService and parse AnalyticsReports
        }
        
    }
}