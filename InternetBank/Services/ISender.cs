namespace InternetBank.Services
{
	public interface ISender
	{
		Task SendEmailAsync(string email, string subject, string message, string name = "");
	}
}
