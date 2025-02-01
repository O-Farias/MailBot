using MailKit.Net.Smtp;
using MimeKit;

namespace MailBot.MailService;

public record EmailSettings
{
    public string SmtpServer { get; init; } = string.Empty;
    public int Port { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FromName { get; init; } = string.Empty;
    public string FromAddress { get; init; } = string.Empty;
}

public class EmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;

            var builder = new BodyBuilder();
            if (message.IsHtml)
                builder.HtmlBody = message.Body;
            else
                builder.TextBody = message.Body;

            foreach (var attachment in message.Attachments)
            {
                builder.Attachments.Add(attachment);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, false);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao enviar e-mail: {ex.Message}", ex);
        }
    }
}