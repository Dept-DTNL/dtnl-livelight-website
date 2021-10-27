using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
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
            
            List<Project> tickedProjects = _projectTimerService.GetTickedProjects(enumerable);

            IEnumerable<Task<AnalyticsReport>> analyticsReportTasks = tickedProjects.Select(
                GetAnalyticsReport);

            AnalyticsReport[] analyticsReports = await Task.WhenAll(analyticsReportTasks.ToArray());
            await UpdateLights(analyticsReports);
        }

        private Task<AnalyticsReport> GetAnalyticsReport(Project project)
        {
            try
            {
                return _gaService.GetAnalyticsReport(project);
            }
            catch (Exception exception)
            {
                //Make sure the worker keeps running thee other projects.
                _logger.LogError(exception, "Could not retrieve Analytics for project {0}:{1}.", project.Id, project.ProjectName);
                return Task.FromResult(new AnalyticsReport()
                {
                    Project = project,
                    ActiveUsers = 0,
                    Conversions = 0
                });
            }
        }

        private async Task UpdateLights(AnalyticsReport[] reports)
        {
            await UpdateLightColors(reports);
            await FlashLightsForConversions(reports);
        }

        private async Task UpdateLightColors(AnalyticsReport[] reports)
        {
            List<Task> tasks = new List<Task>(reports.Length);
            foreach (AnalyticsReport report in reports)
            {
                foreach (ILight light in report.Project.GetLights())
                {
                    switch (light)
                    {
                        case LifxLight lifx:
                            if (!lifx.Active)
                                break;
                            Task lifxTask = LifxLightService.UpdateLightColors(lifx, report.ActiveUsers);
                            tasks.Add(lifxTask);
                            break;
                        default:
                            Project project = report.Project;
                            _logger.LogError("Unknown light type for project {0}:{1}.", project.Id, project.ProjectName);
                            break;
                    }
                }
            }

            await Task.WhenAll(tasks);
        }

        private async Task FlashLightsForConversions(AnalyticsReport[] reports)
        {
            List<Task> tasks = new List<Task>();
            foreach (AnalyticsReport report in reports)
            {
                int flashes = report.Conversions / Math.Max(report.Project.ConversionDivision, 1);

                foreach (ILight light in report.Project.GetLights())
                {
                    switch (light)
                    {
                        case LifxLight lifx:
                            if (!lifx.Active)
                                break;
                            Task lifxTask = LifxLightService.FlashLightForConversions(lifx, flashes,
                                report.Project.PollingTimeInMinutes);
                            tasks.Add(lifxTask);
                            break;
                        default:
                            Project project = report.Project;
                            _logger.LogError("Unknown light type for project {0}:{1}.", project.Id, project.ProjectName);
                            break;
                    }
                }
            }

            await Task.WhenAll(tasks);
        }
        
    }
}