using InternetBank.Models;

namespace InternetBank.Services
{
    public class CodeGeneratingService
    {
        private readonly AppDbContext _dbContext = new();
        public static string Code {  get; set; }
        public static User User { get; set; }
        public CodeGeneratingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CodeGenerate()
        {
            Code = new Random().Next(100000, 999999).ToString();
        }
    }
}
