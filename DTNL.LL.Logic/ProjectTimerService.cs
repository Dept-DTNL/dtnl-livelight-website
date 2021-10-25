using System;
using System.Collections.Generic;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic
{
    /// <summary>
    /// This class should be registered as singleton so the secondsPassed are updated properly.
    /// </summary>
    public class ProjectTimerService
    {

        private int _secondsPassed;
        private readonly int _tickTime;

        private const int SecondsPerMinute = 60;

        private readonly HashSet<int> _sleepingProjectIds = new();

        public ProjectTimerService(IOptions<ServiceWorkerOptions> options)
        {
            _tickTime = options.Value.TickTimeInSeconds;
        }

        /// <summary>
        /// This method updates the internal list of projects that go into sleep mode. This should be called before GetTickedProjects as that method will filter out all sleeping methods.
        /// </summary>
        /// <param name="projects">A list of all projects that should be updated in the internal database.</param>
        /// <returns>A list of all projects that will go to sleep mode this tick. Lamps in this list should be turned off.</returns>
        public List<Project> UpdateSleepingProjectList(IEnumerable<Project> projects)
        {
            List<Project> newSleepingProjects = new List<Project>();
            foreach (Project project in projects)
            {
                if (!project.TimeRangeEnabled)
                {
                    // In case a setting has changed, make sure project is not in the disabled list.
                    if (_sleepingProjectIds.Contains(project.Id))
                        _sleepingProjectIds.Remove(project.Id);
                    continue;
                }

                bool awake = IsTimeOfDayBetween(DateTime.Now, project.TimeRangeStart, project.TimeRangeEnd);

                if (!awake)
                {
                    if (!_sleepingProjectIds.Contains(project.Id))
                        newSleepingProjects.Add(project);

                    _sleepingProjectIds.Add(project.Id);
                    continue;
                }

                if(_sleepingProjectIds.Contains(project.Id))
                    _sleepingProjectIds.Remove(project.Id);
            }

            return newSleepingProjects;
        }

        /// <summary>
        /// Returns a list with the projects that should be polled this tick.
        /// Each time this method is called the internal timer will be raised with set amount of seconds in the ServiceWorker options.
        /// </summary>
        /// <param name="projects"></param>
        /// <returns>Returns a list of project whose light and analytics should updated.</returns>
        public List<Project> GetTickedProjects(IEnumerable<Project> projects)
        {
            _secondsPassed += _tickTime;

            // Make sure an overflow does not happen.
            _secondsPassed = Math.Max(0, _secondsPassed);
            List<Project> tickedProjects = new List<Project>();

            foreach (Project project in projects)
            {
                int pollingTimeInSeconds = project.PollingTimeInMinutes * SecondsPerMinute;
                if(_secondsPassed % pollingTimeInSeconds != 0)
                    continue;

                tickedProjects.Add(project);
            }

            return tickedProjects;
        }

        public static bool IsTimeOfDayBetween(DateTime time,
            TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime < startTime)
                return time.TimeOfDay <= endTime || time.TimeOfDay >= startTime;

            return time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
        }

    }
}