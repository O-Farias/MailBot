using MailKit.Net.Smtp;
using MimeKit;
using MailBot.Models;

namespace MailBot.MailService;

public class EmailSender
{
    private readonly EmailSettings _settings;
    private readonly TemplateService _templateService;

    public EmailSender(EmailSettings settings, TemplateService templateService)
    {
        _settings = settings;
        _templateService = templateService;
    }

    public async Task SendEmailAsync(EmailMessage message)
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

    public async Task SendEmailWithTemplateAsync(string to, string templateName, Dictionary<string, string> variables)
    {
        var message = _templateService.ProcessTemplate(templateName, variables);
        message.To = to;
        await SendEmailAsync(message);
    }
}