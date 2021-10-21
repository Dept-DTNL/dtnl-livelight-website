using System;
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

        private const int SecondsInAMinute = 60;

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

            var enumerable = projects.ToList();
            var newSleepingProjects = _projectTimerService.UpdateSleepingProjectList(enumerable);

            await TurnOffLights(newSleepingProjects);

            var tickedProjects = _projectTimerService.GetTickedProjects(enumerable);

            IEnumerable<Task<AnalyticsReport>> analyticsReportTasks = tickedProjects.Select(
                project => _gaService.GetAnalyticsReport(project));

            var analyticsReports = await Task.WhenAll(analyticsReportTasks.ToArray());
            await UpdateLights(analyticsReports);
        }

        private async Task TurnOffLights(List<Project> projects)
        {
            var tasks = new List<Task>(projects.Count);
            tasks.AddRange(projects.Select(LifxService.DisableLightsAsync));
            await Task.WhenAll(tasks);
        }

        private async Task UpdateLights(AnalyticsReport[] reports)
        {
            await UpdateLightColors(reports);
            await FlashLightsForConversions(reports);
        }

        private LampColor GetActivityColor(AnalyticsReport report)
        {
            var users = report.ActiveUsers;
            var project = report.Project;
            if (project.HighTrafficAmount <= users)
            {
                return new LampColor()
                {
                    Color = project.HighTrafficColor,
                    Brightness = project.HighTrafficBrightness
                };
            } 
            if (project.MediumTrafficAmount <= users)
            {
                return new LampColor()
                {
                    Color = project.MediumTrafficColor,
                    Brightness = project.MediumTrafficBrightness
                };
            }

            return new LampColor()
            {
                Color = project.LowTrafficColor,
                Brightness = project.LowTrafficBrightness
            };
        }

        private async Task UpdateLightColors(AnalyticsReport[] reports)
        {
            var tasks = new List<Task>(reports.Length);
            foreach (var report in reports)
            {
                var color = GetActivityColor(report);
                tasks.Add(LifxService.SetLightsColorAsync(report.Project, color));
            }

            await Task.WhenAll(tasks);
        }

        private async Task FlashLightsForConversions(AnalyticsReport[] reports)
        {
            var tasks = new List<Task>();
            foreach (var report in reports)
            {
                if(report.Conversions == 0)
                    continue;

                var project = report.Project;

                // Makes sure the lamp doesn't flash more than the time it takes for the next polling.
                var maxAmountOfCycles = (project.PollingTimeInMinutes * SecondsInAMinute) / project.ConversionPeriod;
                var maxAmountOfCyclesRounded = Convert.ToInt32(maxAmountOfCycles);
                var cycles = Math.Min(report.Conversions, maxAmountOfCyclesRounded);

                tasks.Add(LifxService.BreatheLightsAsync(project, project.ConversionColor, cycles, project.ConversionPeriod));
            }

            await Task.WhenAll(tasks);
        }
        
    }
}