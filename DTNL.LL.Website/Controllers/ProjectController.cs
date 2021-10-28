﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Logic;
using DTNL.LL.Models;
using Microsoft.AspNetCore.Mvc;
using DTNL.LL.Website.Models;
using Google.Apis.Util;

namespace DTNL.LL.Website.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly AuthService _authService;
        private readonly LifxLightDbService _lifxLightService;

        public ProjectController(ProjectService projectService, AuthService authService, LifxLightDbService lifxLightService)
        {
            _projectService = projectService;
            _authService = authService;
            _lifxLightService = lifxLightService;
        }

        // Shows view of index with search functionality and overview of all projects
        [HttpGet]
        [Route("project")]
        public IActionResult Index()
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");
                
            return View(GetAllProjectDTOs());
        }

        // Filtered list of project list
        [HttpPost]
        [Route("project")]
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
                        projects = projects.FindAll(p => p.CustomerName.Contains(searchString));
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

        // Shows view of create project
        [HttpGet]
        [Route("project/create-project")]
        public IActionResult CreateProject()
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");

            return View(new ProjectDTO()
            {
                Active = true,
                PollingTimeInMinutes = 1,
                AnalyticsVersion = AnalyticsVersion.V4
            });
        }

        // Runs when create button is presses
        [HttpPost]
        [Route("project/create-project")]
        public async Task<ActionResult> CreateProject([FromForm]ProjectDTO project)
        {
            try
            {
                if (project.AnalyticsVersion.Equals(AnalyticsVersion.V3) 
                    && (project.GaProperty.Length < 3 || !project.GaProperty.StartsWith("ga:")))
                    return View(project);

                await _projectService.AddProjectAsync(ProjectDTO.TurnProjectDTOToProject(project));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message + "Something went wrong when creating your project";
                return View();
            }
        }

        // Shows view of create light
        [HttpGet]
        [Route("project/{projectId}/create-light")]
        public IActionResult CreateLight()
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");

            return View(new AllLights()
            {
                LifxLightDto = new LifxLightDTO()
                {
                    LowTrafficColor = "red",
                    LowTrafficBrightness = 0.5,
                    MediumTrafficColor = "orange",
                    MediumTrafficBrightness = 0.5,
                    HighTrafficColor = "green",
                    HighTrafficBrightness = 0.5,
                    MediumTrafficAmount = 5,
                    HighTrafficAmount = 10,
                    ConversionColor = "blue",
                    ConversionCycle = 1,
                    ConversionPeriod = 20,
                    TimeRangeEnabled = true,
                    TimeRangeStart = new DateTime(1, 1, 1, 9, 0, 0),
                    TimeRangeEnd = new DateTime(1, 1, 1, 17, 0, 0)
                }
            });
        }

        // Runs when create button is presses
        [HttpPost]
        [Route("project/{projectId}/create-light")]
        public async Task<ActionResult> CreateLight(int? projectId, [FromForm] AllLights allLights)
        {
            try
            {
                LifxLight light = LifxLightDTO.LifxLightDTOToLifxLight(allLights.LifxLightDto);
                light.Project = await _projectService.FindProjectByIdAsync(projectId.Value);

                await _lifxLightService.CreateLifxLight(light);

                switch (@ViewBag.Light)
                {
                    case "LIFX":
                        await _lifxLightService.CreateLifxLight(LifxLightDTO.LifxLightDTOToLifxLight(allLights.LifxLightDto));
                        break;
                    default:
                        break;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message + "Something went wrong when creating your light";
                return View();
            }
        }


        // Shows view to edit a specific project
        [HttpGet]
        [Route("project/edit-project/{id}")]
        public async Task<IActionResult> EditProject(int? id)
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");

            if (id is null)
            {
                ViewBag.ErrorMessage = "No ids given";
                return View();
            }


            Project projectToUpdate =  _projectService.FindProjectByIdWithLights(id.Value);

            if (projectToUpdate is null)
            {
                ViewBag.ErrorMessage = "Id not found";
                return View();
            }

            //TODO: Change Uuid result
            return View(ProjectDTO.TurnProjectToProjectDTO(projectToUpdate));
        }

        // Updates project
        [HttpPost]
        [Route("project/edit-project/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(int? id, [FromForm] ProjectDTO newValues)
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

        // Shows view to edit a specific project
        [HttpGet]
        [Route("project/edit-project/{projectID}/edit-light/{uuid}")]
        public async Task<IActionResult> EditLight(string uuid)
        {
            if (!_authService.IsLoggedIn())
                return RedirectToAction("Index", "Home");

            if (uuid is null)
            {
                ViewBag.ErrorMessage = "No ids given";
                return View();
            }


            AllLights allLights = new AllLights();
            ILight iLight = _lifxLightService.FindByUuid(uuid).Result;

            if (iLight is null)
            {
                ViewBag.ErrorMessage = "Uuid not found";
                return View();
            }

            allLights.LifxLightDto = LifxLightDTO.LifxLightToLifxLightDTO((LifxLight)iLight);
            
            return View(allLights);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("project/edit-project/{projectId}/edit-light/{uuid}")]
        public async Task<IActionResult> EditLight(string uuid, [FromForm] AllLights newValues)
        {
            if (uuid is null)
            {
                ViewBag.ErrorMessage = "No id given";
                return View();
            }

            if (newValues is null)
            {
                ViewBag.ErrorMessage = "No project given";
                return View();
            }

            await _lifxLightService.Update(uuid, LifxLightDTO.LifxLightDTOToLifxLight(newValues.LifxLightDto));

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
                if (project.LifxLights is null) project.LifxLights = new List<LifxLight>();
                projectViewModels.Add(ProjectDTO.TurnProjectToProjectDTO(project));
            }

            return projectViewModels;
        }
    }
}
