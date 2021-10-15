using System;
using DTNL.LL.DAL;
using DTNL.LL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

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

        public async Task UpdateAccountTokenAsync(int projectId, string token)
        {
            string statusCode;

            using (var client = new HttpClient())
            {
                var url = "https://api.lifx.com/v1/lights/all";
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await client.GetAsync(url);
                statusCode = response.StatusCode.ToString();
            }

            if (statusCode is not "OK")
            {
                throw new Exception("Key given is wrong: " + statusCode);
            }

            Project project = await FindProjectByIdAsync(projectId);
            project.ApiKey = token;

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
