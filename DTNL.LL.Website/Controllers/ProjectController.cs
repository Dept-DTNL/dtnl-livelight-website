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
        // Shows view of index
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Project/Create
        // Shows view of create project
        [HttpGet]
        public IActionResult Create()
        {
            return View();
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
                    await _projectService.AddProjectAsync(new Project()
                    {
                        ProjectName = project.ProjectName,
                        CustomerName = project.CustomerName,
                        Active = project.Active
                    }) ;

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ViewBag.ErrorMessage = "Something went wrong when creating your project";
                return View();
            }

            return View();
        }

        
        // GET: Project/Search
        // View of Search, with a filled project list
        [HttpGet]
        public IActionResult Search()
        {
            return View(GetAllProjectDTOs());
        }

        // POST: Project/Search
        // Filtered list of project list
        [HttpPost]
        public IActionResult Search(string editFilter, string searchString)
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
                        if (int.TryParse(searchString, out var id))
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
            return new ProjectDTO()
            {
                ProjectName = project.ProjectName,
                Active = project.Active,
                CustomerName = project.CustomerName,
                Id = project.Id
            };
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

            var projectToUpdate = await _projectService.FindProjectByIdAsync(id.Value);

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

            await _projectService.UpdateAsync(id.Value, new Project()
            {
                Id = id.Value,
                Active = newValues.Active,
                ProjectName = newValues.ProjectName,
                CustomerName = newValues.CustomerName
            });

            ViewBag.ShowDialog = true;
            return RedirectToAction("Search", "Project");
        }
    }
}
