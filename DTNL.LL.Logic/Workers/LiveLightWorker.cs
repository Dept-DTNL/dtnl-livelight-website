using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DTNL.LL.Logic.Analytics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DTNL.LL.Logic.Workers
{
    public class LiveLightWorker : BackgroundService
    {
        //Todo: Make config value
        public const int ScanDelayTimeInSeconds = 60;
        public const int MilliSecondsInASecond = 1000;
        public const int SecondsInAMinute = 60;

        private readonly ILogger<LiveLightWorker> _logger;
        private readonly GaService _gaService;
        private readonly IServiceScopeFactory _scopeFactory;

        public LiveLightWorker(ILogger<LiveLightWorker> logger, GaService gaService, IServiceScopeFactory scopeFactory, V3Analytics v3)
        {
            _logger = logger;
            _gaService = gaService;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var startTime = DateTime.Now;
                try
                {
                    await ProcessLiveLightProjects();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error Processing Projects.");
                }

                var endTime = DateTime.Now;

                var timeDif = endTime.Subtract(startTime);
                var timeUntilNextScan = ScanDelayTimeInSeconds - timeDif.TotalSeconds;
                await Task.Delay((int)(timeUntilNextScan * MilliSecondsInASecond), cancellationToken);
            }
        }

        private async Task ProcessLiveLightProjects()
        {
            using var scope = _scopeFactory.CreateScope();
            var projectService = scope.ServiceProvider.GetRequiredService<ProjectService>();
            var projects = await projectService.GetActiveProjects();
            var analyticsReports = projects.Select(
                project => _gaService.GetAnalyticsTrafficReport(project.GAProperty, ScanDelayTimeInSeconds * SecondsInAMinute, project.Id));

            await Task.WhenAll(analyticsReports.ToArray());
            foreach (var analyticsReport in analyticsReports)
            {
                //Handle Lamp manipulation
            }
        }
    }
}