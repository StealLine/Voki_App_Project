using AuthService.Application.abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SharedKernel.common.app_users;

namespace AuthService.Infrastructure.email_service;

internal class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly FrontendConfig _frontendConfig;

    public EmailService(EmailServiceConfig config, FrontendConfig frontendConfig) {
        _host = config.Host;
        _port = config.Port;
        _username = config.Username;
        _password = config.Password;
        _frontendConfig = frontendConfig;
    }

    public Task<ErrOrNothing> SendRegistrationConfirmationLink(
        Email email,
        UserUniqueName userUniqueName,
        UnconfirmedUserId userId,
        string confirmationCode,
        DateTime expirationDate
    ) {
        string link =
            $"{_frontendConfig.BaseUrl}/{_frontendConfig.ConfirmRegistrationPath}/{userId}/{confirmationCode}";

        string subject = "Confirm your email address";

        string body = $"""
                       <div style="font-family: Arial, sans-serif; line-height: 1.6; color: #1e293b;">
                           <h2 style="color: #4f46e5;">Welcome, {userUniqueName}!</h2>
                           <p>
                               Thank you for creating an account. Please confirm your email address to activate it.
                           </p>
                           <p style="margin: 1.5rem 0;">
                               <a href="{link}"
                                  style="background-color: #4f46e5; color: #ffffff; text-decoration: none;
                                         padding: 0.6rem 1.2rem; border-radius: 6px; font-weight: bold;">
                                   Confirm Email
                               </a>
                           </p>
                           <p style="font-size: 0.9rem; color: #475569;">
                               This link will expire on {expirationDate:MMMM d, yyyy, HH:mm} (UTC).
                           </p>
                           <hr style="border: none; border-top: 1px solid #e2e8f0; margin: 1.5rem 0;" />
                           <p style="font-size: 0.85rem; color: #64748b;">
                               If the button above does not work, copy and paste this link into your browser:<br />
                               <a href="{link}" style="color: #4f46e5;">{link}</a>
                           </p>
                       </div>
                       """;

        return SendEmailWithHtmlBody(to: email, subject, body);
    }

    private async Task<ErrOrNothing> SendEmailWithHtmlBody(
        Email to, string subject, string body
    ) {
        try {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Vokimi", _username));
            message.To.Add(new MailboxAddress("", to.ToString()));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = await ConfigureSmtpClient();
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return ErrOrNothing.Nothing;
        }
        catch {
            return new Err("Could not establish connection to send an email");
        }
    }

	private async Task<SmtpClient> ConfigureSmtpClient()
	{
		var client = new SmtpClient();
		Console.WriteLine($"SMTP HOST: {_host}");
		Console.WriteLine($"SMTP PORT: {_port}");
		try
		{
			await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
			Console.WriteLine("Connected successfully");
			await client.AuthenticateAsync(_username, _password);
			Console.WriteLine("Authenticated successfully");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"SMTP Error: {ex.GetType().Name}: {ex.Message}");
			throw;
		}
		return client;
	}
}