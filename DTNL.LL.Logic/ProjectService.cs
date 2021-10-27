using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DTNL.LL.DAL;
using DTNL.LL.Models;

namespace DTNL.LL.Logic
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddProjectAsync(Project project)
        {
            _unitOfWork.Projects.AddAsync(project);
            return _unitOfWork.CommitAsync();
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
    }
}
