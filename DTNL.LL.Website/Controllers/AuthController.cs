using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DTNL.LL.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace DTNL.LL.Website.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = Url.Action("Index", "Home");

            if (_authService.IsLoggedIn())
                return new RedirectResult(returnUrl);

            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = returnUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}