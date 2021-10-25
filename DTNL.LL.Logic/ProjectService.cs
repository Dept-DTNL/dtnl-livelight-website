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
            project.Uuid = Guid.NewGuid();
            project.GuideEnabled = true;
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CommitAsync();
        }

        public ValueTask<Project> FindProjectByIdAsync(int id)
        {
            return _unitOfWork.Projects.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int oldProjectId, Project newValues)
        {
            Project project = await FindProjectByIdAsync(oldProjectId);

            project.Active = newValues.Active;
            if (newValues.CustomerName is not null) project.CustomerName = newValues.CustomerName;
            if (newValues.ProjectName is not null) project.ProjectName = newValues.ProjectName;

            project.TimeRangeEnabled = newValues.TimeRangeEnabled;
            project.TimeRangeStart = newValues.TimeRangeStart;
            project.TimeRangeEnd = newValues.TimeRangeEnd;

            if (newValues.MediumTrafficAmount > 0 || newValues.MediumTrafficAmount < newValues.HighTrafficAmount) project.MediumTrafficAmount = newValues.MediumTrafficAmount;
            if (newValues.MediumTrafficAmount > 0 || newValues.MediumTrafficAmount > newValues.HighTrafficAmount) project.HighTrafficAmount = newValues.HighTrafficAmount;

            if (newValues.LowTrafficColor is not null) project.LowTrafficColor = newValues.LowTrafficColor;
            project.LowTrafficBrightness = newValues.LowTrafficBrightness;
            if (newValues.MediumTrafficColor is not null) project.MediumTrafficColor = newValues.MediumTrafficColor;
            project.MediumTrafficBrightness = newValues.MediumTrafficBrightness;
            if (newValues.HighTrafficColor is not null) project.HighTrafficColor = newValues.HighTrafficColor;
            project.HighTrafficBrightness = newValues.HighTrafficBrightness;

            project.GuideEnabled = newValues.GuideEnabled;

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

        public IEnumerable<Project> GetProjectsWithSpecificCustomerName(string customerName)
        {
            return GetSpecifiedProjects(p => p.ProjectName.Contains(customerName));
        }

        public IEnumerable<Project> GetProjectsWithSpecificProjectName(string projectName)
        {
            return GetSpecifiedProjects(p => p.ProjectName.Contains(projectName));
        }

        public async Task<IEnumerable<Project>> GetActiveProjects()
        {
            return await _unitOfWork.Projects.GetActiveProjectsAsync();
        }
        public async Task UpdateApiToken(string projectUuid, string token)
        {
            Project project = GetByUuid(projectUuid);
            project.LifxApiKey = token;
            project.GuideEnabled = false;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.CommitAsync();
        }

        public Project GetByUuid(string uuid)
        {
            return GetSpecifiedProjects(p => p.Uuid.ToString() == uuid).FirstOrDefault();
        }

    }
}
