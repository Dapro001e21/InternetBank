using InternetBank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace InternetBank.Areas.Identity.Pages.Account
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Model model { get; set; } = new Model();
        public string ErrorMessage { get; set; } = "";

        private readonly AuthenticationService _authService;
        private readonly IHttpContextAccessor _accessor;
        public IndexModel(AuthenticationService authService, IHttpContextAccessor accessor)
        {
            _authService = authService;
            _accessor = accessor;
        }

        public IActionResult OnGetAsync()
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.UserLoginAsync(model.Email, model.Password);
                ErrorMessage = result.Message;
                return result.Success ? Redirect("/Account") : Page();
            }
            return Page();
        }

        public class Person
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public class Model
        {
            [Required(ErrorMessage = "Поле почта является обязательным.")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Поле пароль является обязательным.")]
            public string Password { get; set; }
        }
    }
}
