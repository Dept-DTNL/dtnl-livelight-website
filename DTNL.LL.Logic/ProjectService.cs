using System;
using DTNL.LL.DAL;
using DTNL.LL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task<Project> FindProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            await _unitOfWork.CommitAsync();

            return project;
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

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            await _unitOfWork.CommitAsync();

            return projects;
        }

        public IEnumerable<Project> GetSpecifiedProjects(Expression<Func<Project, bool>> expression)
        {
            var projects =  _unitOfWork.Projects.Find(expression);
            _unitOfWork.CommitAsync();

            return projects;
        }

        public IEnumerable<Project> GetProjectsWithSpecificCustomerName(string customerName)
        {
            return GetSpecifiedProjects(p => p.ProjectName.Contains(customerName));
        }

        public IEnumerable<Project> GetProjectsWithSpecificProjectName(string projectName)
        {
            return GetSpecifiedProjects(p => p.ProjectName.Contains(projectName));
        }
    }
}
