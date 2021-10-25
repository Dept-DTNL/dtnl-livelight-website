using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DTNL.LL.Logic;
using DTNL.LL.Models;

namespace DTNL.LL.Website.Controllers
{
    public class InstallGuideController : Controller
    {
        private readonly ProjectService _projectService;

        public InstallGuideController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("projects/{projectUuid}/add-lamp")]
        public ActionResult Index(string projectUuid)
        {
            if (projectUuid is null)
            {
                return RedirectToAction("Error", "Home");
            }

            Project project = _projectService.GetByUuid(projectUuid);
            if (project is null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!project.GuideEnabled)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ProjectUuid = projectUuid;
            return View();
        }

        [HttpGet]
        [Route("projects/{projectUuid}/add-lamp/lamp-setup")]
        public ActionResult SetUpLamp(string projectUuid)
        {
            ViewBag.ProjectUuid = projectUuid;
            return View();
        }

        [HttpGet]
        [Route("projects/{projectUuid}/add-lamp/authorize-account")]
        public ActionResult AuthorizeAccount(string projectUuid)
        {
            ViewBag.ProjectUuid = projectUuid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectUuid}/add-lamp/authorize-account")]
        public async Task<ActionResult> AuthorizeAccount(string projectUuid, [FromForm] string key)
        {
            if (key is null)
            {
                ViewBag.ErrorMessage = "Please insert a key";
                return View();
            }

            if (projectUuid is null)
            {
                ViewBag.ErrorMessage = "No project id";
                return View();
            }
            
            try
            {
                await _projectService.UpdateApiToken(projectUuid, key);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }

            return Redirect($"/projects/{projectUuid}/add-lamp/thank-you");
           
        }


        [HttpGet]
        [Route("projects/{projectUuid}/add-lamp/thank-you")]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
