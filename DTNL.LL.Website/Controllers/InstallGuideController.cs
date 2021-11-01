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
        private readonly LifxLightDbService _lifxLightDbService;

        public InstallGuideController(ProjectService projectService, LifxLightDbService lifxLightDbService)
        {
            _projectService = projectService;
            _lifxLightDbService = lifxLightDbService;
        }

        [HttpGet]
        [Route("livelight-setup/{lightUuid}")]
        public ActionResult Index(string lightUuid)
        {
            if (lightUuid is null)
            {
                return RedirectToAction("Error", "Home");
            }

            //TODO: Change to Ilight, not LifxLight
            LifxLight light = _lifxLightDbService.FindByUuid(lightUuid);

            if (light == null || !light.GuideEnabled)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.LightUuid = light.Uuid;
            return View();
        }

        //LIFX LAMP SET UP
        [HttpGet]
        [Route("livelight-setup/{lightUuid}/LIFX/setup-account")]
        public ActionResult SetUpAccountLifx(string lightUuid)
        {
            ViewBag.LightUuid = lightUuid;
            return View();
        }

        [HttpGet]
        [Route("livelight-setup/{lightUuid}/LIFX/add-lamp")]
        public ActionResult AddLampLifx(string lightUuid)
        {
            ViewBag.LightUuid = lightUuid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("livelight-setup/{lightUuid}/LIFX/add-lamp")]
        public async Task<ActionResult> AddLampLifx(string lightUuid, [FromForm] string key)
        {
            if (key is null)
            {
                ViewBag.ErrorMessage = "Please insert a key";
                return View();
            }

            if (lightUuid is null)
            {
                ViewBag.ErrorMessage = "No project id";
                return View();
            }
            
            try
            {
                await _lifxLightDbService.UpdateKey(lightUuid, key);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }

            return Redirect($"/livelight-setup/{lightUuid}/thank-you");
           
        }


        [HttpGet]
        [Route("livelight-setup/{lightUuid}/thank-you")]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
