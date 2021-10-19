using System;
using System.Collections.Generic;
using DTNL.LL.Logic.Options;
using DTNL.LL.Models;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic
{
    public class ProjectTimerService
    {

        private int _secondsPassed = 0;
        private readonly int _tickTime;

        private const int SecondsPerMinute = 60;

        private List<int> _sleepingProjectIds = new();

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
            var newSleepingProjects = new List<Project>();
            foreach (var project in projects)
            {
                //Todo: Set bool if project is awake
                var awake = false;

                if (!awake && !_sleepingProjectIds.Contains(project.Id))
                {
                    newSleepingProjects.Add(project);
                    _sleepingProjectIds.Add(project.Id);
                }
                if(!awake)
                    continue;

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
        /// <returns></returns>
        public List<Project> GetTickedProjects(IEnumerable<Project> projects)
        {
            _secondsPassed += _tickTime;
            var tickedProjects = new List<Project>();

            foreach (var project in projects)
            {
                var pollingTimeInSeconds = project.PollingTimeInMinutes * SecondsPerMinute;
                if(_secondsPassed % pollingTimeInSeconds != 0)
                    continue;
                tickedProjects.Add(project);
            }

            return tickedProjects;
        }


    }
}