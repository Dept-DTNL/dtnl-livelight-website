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

        private readonly ILogger<LiveLightService> _logger;
        private readonly GaService _gaService;
        private readonly ProjectService _projectService;
        private readonly ProjectTimerService _projectTimerService;

        public LiveLightService(ILogger<LiveLightService> logger, IOptions<ServiceWorkerOptions> options, GaService gaService, ProjectService projectService, ProjectTimerService projectTimerService)
        {
            _logger = logger;
            _gaService = gaService;
            _projectService = projectService;
            _projectTimerService = projectTimerService;
        }

        public async Task ProcessLiveLights()
        {
            var projects = await _projectService.GetActiveProjects();

            var enumerable = projects.ToList();
            _projectTimerService.UpdateSleepingProjectList(enumerable);
            var tickedProjects = _projectTimerService.GetTickedProjects(enumerable);

            var analyticsReportTasks = tickedProjects.Select(
                project => _gaService.GetAnalyticsReport(project));

            var analyticsReports = await Task.WhenAll(analyticsReportTasks.ToArray());
            // Todo Create a LightService and parse AnalyticsReports
        }
        
    }
}