using InternetBank.Models;
using InternetBank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace InternetBank.Areas.Identity.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        AuthenticationService _authService;
        ISender _emailService;
        CodeGeneratingService _codeService;
        ProtectedSessionStorage _store;
        private static User user;
        private int _code;
        public string Email { get; set; }
        public int CodeForCompare { get; set; }
        public string ErrorMessage { get; set; }
        public ForgetPasswordModel(AuthenticationService authService, ISender emailService, CodeGeneratingService codeService, ProtectedSessionStorage store)
        {
            _authService = authService;
            _emailService = emailService;
            _codeService = codeService;
            _store = store;
        }
        
        public void OnGet()
        {
        }
        public async Task OnPostAsync(string email)
        {
            var result = await _authService.EmailIsExist(email);
            CodeGeneratingService.User = await _authService.GetUserByEmailAsync(email);
            code_form
            ErrorMessage = result.Message;
        }

        public async Task OnPostSendCode()
        {
            if(CodeGeneratingService.User != null)
            {
                _codeService.CodeGenerate();
                await _emailService.SendEmailAsync(CodeGeneratingService.User.Email, "код для смены пароля", CodeGeneratingService.Code);

            }
        }
        public async Task OnPostCodeCompare(string code)
        {
            if(code == CodeGeneratingService.Code)
            {
                Console.WriteLine(code);
            }
        }
    }
}
