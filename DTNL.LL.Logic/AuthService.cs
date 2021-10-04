using Microsoft.AspNetCore.Http;

namespace DTNL.LL.Logic
{
    public class AuthService
    {

        private readonly HttpContext _httpContext;

        public AuthService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext.HttpContext;
        }

        public bool IsLoggedIn()
        {
            return _httpContext.User.Identity!.IsAuthenticated;
        }
    }
}