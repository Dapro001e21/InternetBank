using InternetBank.Models;
using InternetBank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;

namespace InternetBank.Areas.Identity.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        UserService _userService;
        IEmailService _emailService;
        public CodeGeneratingService _codeGeneratingService { get; set; }
        public string ErrorMessage { get; set; }

        public ForgetPasswordModel(UserService userService, IEmailService emailService, CodeGeneratingService codeGeneratingService)
        {
            _userService = userService;
            _emailService = emailService;
            _codeGeneratingService = codeGeneratingService;
        }

        public async Task OnPostSendCode(string email)
        {
            OnLoadData();
            var result = await _userService.EmailIsExistAsync(email);
            if (result.Success)
            {
                _codeGeneratingService.Email = email;
                _codeGeneratingService.CodeGenerate();
                await _emailService.SendEmailAsync(email, "Код для смены пароля", _codeGeneratingService.Code, "InternetBank");
            }
            ErrorMessage = result.Message;
            OnSaveData();
        }
        public void OnPostCodeCompare(string code)
        {
            OnLoadData();
            if (code == _codeGeneratingService.Code)
            {
                _codeGeneratingService.IsCodeConfirm = true;
            }
            else
            {
                ErrorMessage = "Код не совпадает!";
            }
            OnSaveData();
        }
        public async Task<IActionResult> OnPostReplacePassword(string newPassword, string confirm_newPassword)
        {
            OnLoadData();
            if (newPassword != confirm_newPassword)
            {
                ErrorMessage = "Пароли не совпадают!";
                return Page();
            }
            var result = await _userService.ChangePasswordAsync(_codeGeneratingService.Email, newPassword);
            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return Page();
            }
            OnClearData();
            return Redirect("/Login");
        }

        private void OnSaveData()
        {
            HttpContext.Session.SetString("CodeGenerating", JsonSerializer.Serialize(_codeGeneratingService));
        }

        private void OnLoadData()
        {
            var value = HttpContext.Session.GetString("CodeGenerating");
            if (value != null){
                _codeGeneratingService = JsonSerializer.Deserialize<CodeGeneratingService>(value);
            }
        }

        private void OnClearData()
        {
            HttpContext.Session.Clear();
        }
    }
}
