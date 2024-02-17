using InternetBank.Models;

namespace InternetBank.Services
{
    public class CodeGeneratingService
    {
        public string Email { get; set; }
        public string Code {  get; set; }
        public bool IsCodeConfirm { get; set; }
        public void CodeGenerate()
        {
            Code = new Random().Next(100000, 999999).ToString();
        }
    }
}
