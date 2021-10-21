using System.Collections.Generic;
using DTNL.LL.DAL;
using DTNL.LL.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using DTNL.LL.Models;

namespace DTNL.LL.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            // Project p = new()
            // {
            //     Id = 0,
            //     ProjectName = "p1",
            //     CustomerName = "Gianni",
            //     Active = true,
            //     TimeRangeEnabled = false,
            //     TimeRangeStart = default,
            //     TimeRangeEnd = default,
            //     PollingTimeInMinutes = 1,
            //     GaProperty = "288717872",
            //     AnalyticsVersion = AnalyticsVersion.V4,
            //     ConversionTags = new List<string>(){"convert"},
            //     LifxApiKey = "cfcd89c1b94fe18c1dbb15f11904182dc39e7a707b923223932ae105a5adc772",
            //     LightGroupName = "LL",
            //     Uuid = default,
            //     GuideEnabled = false,
            //     LowTrafficColor = "red",
            //     LowTrafficBrightness = 0.5d,
            //     MediumTrafficAmount = 1,
            //     MediumTrafficColor = "yellow",
            //     MediumTrafficBrightness = 0.5d,
            //     HighTrafficColor = "green",
            //     HighTrafficBrightness = 0.5f,
            //     HighTrafficAmount = 3,
            //     ConversionCycle = 0,
            //     ConversionPeriod = 1,
            //     ConversionColor = "white saturation:0.0 brightness:0.5"
            // };
            //
            // await _unitOfWork.Projects.AddAsync(p);
            // await _unitOfWork.CommitAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
