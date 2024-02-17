
using InternetBank.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InternetBank.Services
{
    public class EmailService : IEmailService
	{
		SmtpConfig _config;
		public EmailService(IOptions<SmtpConfig> options)
		{
			_config = options.Value;
		}

		public async Task SendEmailAsync(string email, string subject, string message, string name = "")
		{
			using var emailMessage = new MimeMessage();

			emailMessage.From.Add(new MailboxAddress(name, _config.UserName));
			emailMessage.To.Add(new MailboxAddress("", email));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync(_config.Host, _config.Port, false);
				await client.AuthenticateAsync(_config.UserName, _config.Password);
				await client.SendAsync(emailMessage);

				await client.DisconnectAsync(true);
			}
		}
	}
}
