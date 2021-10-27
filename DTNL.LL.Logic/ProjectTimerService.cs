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

        public ProjectTimerService(IOptions<ServiceWorkerOptions> options)
        {
            _tickTime = options.Value.TickTimeInSeconds;
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

    }
}