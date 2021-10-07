using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DTNL.LL.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DTNL.LL.Logic.Workers
{
    public class LiveLightWorker : BackgroundService
    {
        //Todo: Make config value
        public const int ScanDelayTimeInSeconds = 60;
        private readonly ILogger<LiveLightWorker> _logger;
        private readonly GaService _gaService;
        private readonly ProjectService _projectService;

        public LiveLightWorker(ILogger<LiveLightWorker> logger, GaService gaService, ProjectService projectService)
        {
            _logger = logger;
            _gaService = gaService;
            _projectService = projectService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var startTime = DateTime.Now;

                await ProcessLiveLightProjects();;
                var endTime = DateTime.Now;

                var timeDif = endTime.Subtract(startTime);
                var timeUntilNextScan = ScanDelayTimeInSeconds - timeDif.TotalSeconds;
                await Task.Delay((int)(timeUntilNextScan * 1000), cancellationToken);
            }
        }

        private async Task ProcessLiveLightProjects()
        {
            var projects = await _projectService.GetActiveProjects();
            var analyticsReports = projects.Select(
                project => _gaService.GetAnalyticsTrafficReport(project.GAProperty, ScanDelayTimeInSeconds * 60, project.Id));

            await Task.WhenAll(analyticsReports.ToArray());
            foreach (var analyticsReport in analyticsReports)
            {
                //Handle Lamp manipulation
            }
        }
    }
}