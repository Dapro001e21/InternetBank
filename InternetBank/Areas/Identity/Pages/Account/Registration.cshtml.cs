using InternetBank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace InternetBank.Areas.Identity.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public Model model { get; set; } = new Model();
        public string ErrorMessage { get; set; } = "";

        private readonly AuthenticationService _authService;
        private readonly IHttpContextAccessor _accessor;
        public RegistrationModel(AuthenticationService authService, IHttpContextAccessor accessor)
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
                var result = await _authService.UserRegistrationAsync(model.Name, model.Email, model.Password, model.RepeatPassword);
                ErrorMessage = result.Message;
                return result.Success ? Redirect("/Account") : Page();
            }
            return Page();
        }

        public class Model
        {
            [Required(ErrorMessage = "Поле имя является обязательным.")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Поле почта является обязательным.")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Поле пароль является обязательным.")]
            public string Password { get; set; }
            [Required(ErrorMessage = "Повторите пароль.")]
            public string RepeatPassword { get; set; }
        }
    }
}
