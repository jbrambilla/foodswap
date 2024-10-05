
using System.Net;
using System.Net.Mail;
using foodswap.Business.Interfaces.Services;
using foodswap.Options;
using Serilog;

namespace foodswap.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettingsOptions _emailSettings;
    public EmailService(IConfiguration configuration)
    {
        _emailSettings = configuration
            .GetSection(EmailSettingsOptions.SectionName)
            .Get<EmailSettingsOptions>()!;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try 
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.UserEmail, _emailSettings.Password)
            };
            
            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            message.To.Add(to);
            message.Sender = new MailAddress(_emailSettings.UserEmail);

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            Log.ForContext("EmailTo", to)
                .ForContext("EmailSubject", subject)
                .ForContext("EmailBody", body)
                .ForContext("Service", "EmailService")
                .Error(ex, "Error sending email");
            throw;
        }
    }
}