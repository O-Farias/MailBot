using MailBot.Models;

namespace MailBot.MailService;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage message);
    Task SendEmailWithTemplateAsync(string to, string templateName, Dictionary<string, string> variables);
}