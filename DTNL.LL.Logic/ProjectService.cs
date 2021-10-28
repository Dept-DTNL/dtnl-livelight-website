using System;
using DTNL.LL.DAL;
using DTNL.LL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DTNL.LL.Logic
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProjectAsync(Project project)
        {
            await _unitOfWork.Projects.AddAsync(project); 
            await _unitOfWork.CommitAsync();
        }

        public ValueTask<Project> FindProjectByIdAsync(int id)
        {
            return _unitOfWork.Projects.GetByIdAsync(id);
        }

        public Project FindProjectByIdWithLights(int id)
        {
            Project project = _unitOfWork.Projects.GetByIdAsync(id).Result;
            project.LifxLights = _unitOfWork.LifxLights.Find(l => l.Project.Id == id).ToList();

            return project;
        }

        public async Task UpdateAsync(int oldProjectId, Project newValues)
        {
            Project project = await FindProjectByIdAsync(oldProjectId);

            project.Active = newValues.Active;
            if (newValues.CustomerName is not null) project.CustomerName = newValues.CustomerName;
            if (newValues.ProjectName is not null) project.ProjectName = newValues.ProjectName;

            project.AnalyticsVersion = newValues.AnalyticsVersion;
            if (newValues.PollingTimeInMinutes > 0) project.PollingTimeInMinutes = newValues.PollingTimeInMinutes;
            
            _unitOfWork.Projects.Update(project);
            await _unitOfWork.CommitAsync();
        }

        public Task<List<Project>> GetAllAsync()
        {
            return _unitOfWork.Projects.GetAllAsync();
        }

        public IEnumerable<Project> GetSpecifiedProjects(Expression<Func<Project, bool>> expression)
        {
            return _unitOfWork.Projects.Find(expression);
        }

        public async Task<IEnumerable<Project>> GetActiveProjects()
        {
            return await _unitOfWork.Projects.GetActiveProjectsAsync();
        }
    }
}
