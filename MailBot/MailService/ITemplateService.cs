using MailBot.Models;

namespace MailBot.MailService;

public interface ITemplateService
{
    void AddTemplate(string name, EmailTemplate template);
    EmailTemplate GetTemplate(string name);
    EmailMessage ProcessTemplate(string templateName, Dictionary<string, string> variables);
}