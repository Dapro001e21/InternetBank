using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternetBank.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpContextAccessor _accessor;
        public LogoutModel(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                await _accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return Redirect("/");
        }
    }
}
