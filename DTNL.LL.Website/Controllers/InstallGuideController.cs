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
        [Route("projects/{projectId}/add-lamp")]
        // GET: InstallGuide
        public ActionResult Index(int? projectId)
        {
            if (projectId is null)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpGet]
        [Route("projects/{projectId}/add-lamp/lamp-setup")]
        // GET: InstallGuide/WizSetUp
        public ActionResult SetUpLamp(int? projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpGet]
        [Route("projects/{projectId}/add-lamp/authorize-account")]
        public ActionResult AuthorizeAccount(int? projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectId}/add-lamp/authorize-account")]
        public async Task<ActionResult> AuthorizeAccount(int? projectId, [FromForm] string? key)
        {
            if (key is null)
            {
                ViewBag.ErrorMessage = "Please insert a key";
                return View();
            }

            if (projectId is null)
            {
                ViewBag.ErrorMessage = "No project id";
                return View();
            }
            
            try
            {
                //await _projectService.UpdateAccountTokenAsync(projectId.Value, key);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }

            return RedirectToAction("AuthorizeLamp", "InstallGuide", new { projectId = projectId });
        }

        [HttpGet]
        [Route("projects/{projectId}/add-lamp/authorize-lamp")]
        public ActionResult AuthorizeLamp(int? projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectId}/add-lamp/authorize-lamp")]
        public async Task<ActionResult> AuthorizeLampAsync(int? projectId, [FromForm] string? key)
        {
            if (key is null)
            {
                ViewBag.ErrorMessage = "Please insert a key";
                return View();
            }

            if (projectId is null)
            {
                ViewBag.ErrorMessage = "No project id";
                return View();
            }

            // Check if the key is valid or not
            if (false)
            {
                ViewBag.ErrorMessage = "Invalid key";
                return View();
            }

            // Check if all values have been inserted
            // TODO: Remove initialization of values like following. This is here now as a placeholder

             
            return RedirectToAction("ThankYou", "InstallGuide", new { projectId = projectId });
        }

        [HttpGet]
        [Route("projects/{projectId}/add-lamp/thank-you")]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
