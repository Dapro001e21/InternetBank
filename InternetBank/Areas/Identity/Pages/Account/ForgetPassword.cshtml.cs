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
        UserService _userService;
        IEmailService _emailService;
        public string ErrorMessage { get; set; }

        public ForgetPasswordModel(UserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task OnPostSendCode(string email)
        {
            var result = await _userService.EmailIsExist(email);
            if (result.Success)
            {
                CodeGeneratingService.Email = email;
                CodeGeneratingService.CodeGenerate();
                await _emailService.SendEmailAsync(email, "Код для смены пароля", CodeGeneratingService.Code);
            }
            ErrorMessage = result.Message;
        }
        public async Task OnPostCodeCompare(string code)
        {
            if(code == CodeGeneratingService.Code)
            {
                CodeGeneratingService.IsCodeConfirm = true;
            }
            else
            {
                ErrorMessage = "Код не совпадает!";
            }
        }
        public async Task<IActionResult> OnPostReplacePassword(string newPassword, string confirm_newPassword)
        {
            if (newPassword != confirm_newPassword)
            {
                ErrorMessage = "Пароли не совпадают!";
                return Page();
            }
            var result = await _userService.ChangePasswordAsync(CodeGeneratingService.Email, newPassword);
            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return Page();
            }
            return Redirect("/Login");
        }

    }
}
