using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic;
using DTNL.LL.Models;
using Microsoft.AspNetCore.Mvc;
using DTNL.LL.Website.Models;

namespace DTNL.LL.Website.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: Project
        // Shows view of index with search functionality and overview of all projects
        [HttpGet]
        public IActionResult Index()
        {
            return View(GetAllProjectDTOs());
        }

        // POST: Project
        // Filtered list of project list
        [HttpPost]
        public IActionResult Index(string editFilter, string searchString)
        {
            List<Project> projects = new List<Project>();

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (editFilter)
                {
                    case "projectName":
                        projects = _projectService.GetProjectsWithSpecificCustomerName(searchString).ToList();
                        break;
                    case "customerName":
                        projects = _projectService.GetProjectsWithSpecificProjectName(searchString).ToList();
                        break;
                    case "id":
                        if (int.TryParse(searchString, out int id))
                            projects.Add(_projectService.FindProjectByIdAsync(id).Result);

                        break;
                }
            }

            if (projects.Count == 0)
            {
                return View(GetAllProjectDTOs());
            }

            List<ProjectDTO> projectViewModels = TurnProjectsToProjectDTOs(projects);

            return View(projectViewModels);
        }

        // GET: Project/Create
        // Shows view of create project
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProjectDTO()
            {
                Active = true,
                HasTimeRange = true,
                TimeRangeStart = new DateTime(1, 1, 1, 9, 0, 0),
                TimeRangeEnd = new DateTime(1, 1, 1, 17, 0, 0)
            });
        }

        // POST: Project/Create
        // Runs when create button is presses
        [HttpPost]
        public async Task<ActionResult> Create([FromForm]ProjectDTO project)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    await _projectService.AddProjectAsync(TurnProjectDTOToProject(project)) ;

                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message + "Something went wrong when creating your project";
                return View();
            }

            return View();
        }

        // Getting ProjectViewModel list of all projects
        private List<ProjectDTO> GetAllProjectDTOs()
        {
            List<Project> projects = _projectService.GetAllAsync().Result.ToList();
            return TurnProjectsToProjectDTOs(projects);
        }

        // Turning Project List into ProjectViewModel List
        private List<ProjectDTO> TurnProjectsToProjectDTOs(List<Project> projects)
        {
            List<ProjectDTO> projectViewModels = new List<ProjectDTO>();
            foreach (Project project in projects)
            {
                projectViewModels.Add(TurnProjectToProjectDTO(project));
            }

            return projectViewModels;
        }

        // Turns Project to a ProjectDTO
        private ProjectDTO TurnProjectToProjectDTO(Project project)
        {

            ProjectDTO dto =  new()
            {
                ProjectName = project.ProjectName,
                Active = project.Active,
                CustomerName = project.CustomerName,
                Id = project.Id,
                // HasTimeRange = project.TimeRangeEnabled,
                // TimeRangeStart = project.TimeRangeStart != null ? new DateTime(1, 1, 1, project.TimeRangeStart.Hours, project.TimeRangeStart.Minutes, project.TimeRangeStart.Seconds) : new DateTime(),
                // TimeRangeEnd = project.TimeRangeEnd != null ? new DateTime(1, 1, 1, project.TimeRangeEnd.Hours, project.TimeRangeEnd.Minutes, project.TimeRangeEnd.Seconds) : new DateTime()
            };

            return dto;
        }

        // Turns Project to a ProjectDTO
        private Project TurnProjectDTOToProject(ProjectDTO dto)
        {
            Project project = new()
            {
                ProjectName = dto.ProjectName,
                Active = dto.Active,
                CustomerName = dto.CustomerName,
                Id = dto.Id,
                // TimeRangeEnabled = dto.HasTimeRange,
                // TimeRangeStart = new TimeSpan(dto.TimeRangeStart.Hour, dto.TimeRangeStart.Minute, dto.TimeRangeStart.Second),
                // TimeRangeEnd = new TimeSpan(dto.TimeRangeEnd.Hour, dto.TimeRangeEnd.Minute, dto.TimeRangeEnd.Second)
            };

            return project;
        }

        // GET: Project/Edit/{id}
        // Shows view to edit a specific project
        [HttpGet]
        [Route("Project/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                ViewBag.ErrorMessage = "No ids given";
                return View();
            }

            Project projectToUpdate = await _projectService.FindProjectByIdAsync(id.Value);

            if (projectToUpdate is null)
            {
                ViewBag.ErrorMessage = "Id not found";
                return View();
            }

            return View(TurnProjectToProjectDTO(projectToUpdate));

        }

        // POST: Project/Edit/{id}
        // Updates project
        [HttpPost]
        [Route("Project/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromForm] ProjectDTO newValues)
        {
            if (id is null)
            {
                ViewBag.ErrorMessage = "No id given";
                return View();
            }


            if (newValues is null)
            {
                ViewBag.ErrorMessage = "No project given";
                return View();
            }

            await _projectService.UpdateAsync(id.Value, TurnProjectDTOToProject(newValues));

            ViewBag.ShowDialog = true;
            return RedirectToAction("Index", "Project");
        }
    }
}
