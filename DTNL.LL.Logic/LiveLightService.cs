using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
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
            IEnumerable<Project> projects = await _projectService.GetActiveProjects();

            List<Project> enumerable = projects.ToList();
            _projectTimerService.UpdateSleepingProjectList(enumerable);
            List<Project> tickedProjects = _projectTimerService.GetTickedProjects(enumerable);

            IEnumerable<Task<AnalyticsReport>> analyticsReportTasks = tickedProjects.Select(
                project => _gaService.GetAnalyticsReport(project));

            AnalyticsReport[] analyticsReports = await Task.WhenAll(analyticsReportTasks.ToArray());
            // Todo Create a LightService and parse AnalyticsReports
        }
        
    }
}