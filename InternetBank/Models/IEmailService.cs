namespace InternetBank.Models
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, string name = "");
    }
}
