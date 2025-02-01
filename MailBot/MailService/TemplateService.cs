using MailBot.Models;

namespace MailBot.MailService;

public class TemplateService : ITemplateService
{
    private readonly Dictionary<string, EmailTemplate> _templates = new();

    public void AddTemplate(string name, EmailTemplate template)
    {
        _templates[name] = template;
    }

    public EmailTemplate GetTemplate(string name)
    {
        if (!_templates.ContainsKey(name))
            throw new KeyNotFoundException($"Template '{name}' não encontrado.");

        return _templates[name];
    }

    public EmailMessage ProcessTemplate(string templateName, Dictionary<string, string> variables)
    {
        var template = GetTemplate(templateName);
        var subject = template.Subject;
        var body = template.Body;

        foreach (var variable in variables)
        {
            subject = subject.Replace($"{{{variable.Key}}}", variable.Value);
            body = body.Replace($"{{{variable.Key}}}", variable.Value);
        }

        return new EmailMessage
        {
            Subject = subject,
            Body = body,
            IsHtml = template.IsHtml
        };
    }
}