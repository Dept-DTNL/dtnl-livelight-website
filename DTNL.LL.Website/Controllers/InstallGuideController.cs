using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DTNL.LL.Logic;
using DTNL.LL.Models;

namespace DTNL.LL.Website.Controllers
{
    public class InstallGuideController : Controller
    {

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
        [Route("projects/{projectId}/add-lamp/wiz-setup")]
        // GET: InstallGuide/WizSetUp
        public ActionResult WizSetUp(int? projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpGet]
        [Route("projects/{projectId}/add-lamp/authorize-wiz")]
        public ActionResult AuthorizeLamp(int? projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectId}/add-lamp/authorize-wiz")]
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
            Lamp lamp = new Lamp()
            {
                AccessToken = key,
                ExpiresAt = DateTime.MinValue,
                RefreshToken = "RefreshToken",
                TokenType = "TokenType"
            };
            
             
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
