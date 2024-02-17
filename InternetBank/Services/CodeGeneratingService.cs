using InternetBank.Models;

namespace InternetBank.Services
{
    public class CodeGeneratingService
    {
        public static string Email { get; set; }
        public static string Code {  get; set; }
        public static bool IsCodeConfirm { get; set; }
        public static void CodeGenerate()
        {
            Code = new Random().Next(100000, 999999).ToString();
        }
    }
}
