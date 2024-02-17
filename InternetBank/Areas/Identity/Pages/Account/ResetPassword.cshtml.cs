using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternetBank.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        public string Code { get; set; }
        public void OnPost()
        {
            Code = new Random().Next(10000, 99999).ToString();
        }
    }
}
