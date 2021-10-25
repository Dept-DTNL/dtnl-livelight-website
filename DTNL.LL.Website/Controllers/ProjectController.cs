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
        private readonly AuthService _authService;

        public ProjectController(ProjectService projectService, AuthService authService)
        {
            _projectService = projectService;
            _authService = authService;
        }

        // GET: Project
        // Shows view of index with search functionality and overview of all projects
        [HttpGet]
        public IActionResult Index()
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");
                
            return View(GetAllProjectDTOs());
        }

        // POST: Project
        // Filtered list of project list
        [HttpPost]
        public IActionResult Index(string editFilter, string searchString)
        {
            List<ProjectDTO> projects = GetAllProjectDTOs();

            switch (editFilter)
            {
                case "projectName":
                    if (!String.IsNullOrEmpty(searchString))
                        projects = projects.FindAll(p => p.ProjectName.Contains(searchString));
                    projects = projects.OrderBy(p => p.ProjectName).ToList();
                    break;
                case "customerName":
                    if (!String.IsNullOrEmpty(searchString))
                        projects = projects.FindAll(p => p.ConversionColor.Contains(searchString));
                    projects = projects.OrderBy(p => p.CustomerName).ToList();
                    break;
                case "id":
                    if (int.TryParse(searchString, out var id) && !String.IsNullOrEmpty(searchString))
                        projects = projects.FindAll(p => p.Id == id);
                    projects = projects.OrderBy(p => p.Id).ToList();
                    break;
            }

            return View(projects);
        }

        // GET: Project/Create
        // Shows view of create project
        [HttpGet]
        public IActionResult Create()
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");

            return View(new ProjectDTO()
            {
                Active = true,
                HasTimeRange = true,
                TimeRangeStart = new DateTime(1, 1, 1, 9, 0, 0),
                TimeRangeEnd = new DateTime(1, 1, 1, 17, 0, 0),
                MediumTrafficAmount = 10,
                HighTrafficAmount = 50,
                LowTrafficColor = "red",
                LowTrafficBrightness = 0.5,
                MediumTrafficColor = "orange",
                MediumTrafficBrightness = 0.5,
                HighTrafficColor = "green",
                HighTrafficBrightness = 0.5,
                ConversionColor = "blue",
                ConversionPeriod = 0.5,
                ConversionCycle = 20
            });
        }

        // POST: Project/Create
        // Runs when create button is presses
        [HttpPost]
        public async Task<ActionResult> Create([FromForm]ProjectDTO project)
        {
            try
            {
                await _projectService.AddProjectAsync(ProjectDTO.TurnProjectDTOToProject(project)) ;
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message + "Something went wrong when creating your project";
                return View();
            }
        }

        // GET: Project/Edit/{id}
        // Shows view to edit a specific project
        [HttpGet]
        [Route("Project/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");

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

            ViewBag.InstallUrl = $"/projects/{_projectService.FindProjectByIdAsync(id.Value).Result.Uuid}/add-lamp";
            return View(ProjectDTO.TurnProjectToProjectDTO(projectToUpdate));

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

            await _projectService.UpdateAsync(id.Value, ProjectDTO.TurnProjectDTOToProject(newValues));

            return RedirectToAction("Index", "Project");
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
                projectViewModels.Add(ProjectDTO.TurnProjectToProjectDTO(project));
            }

            return projectViewModels;
        }
    }
}
