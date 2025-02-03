using MailKit.Net.Smtp;
using MimeKit;
using MailBot.Models;
using Microsoft.Extensions.Logging;

namespace MailBot.MailService;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ITemplateService _templateService;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(EmailSettings settings, ITemplateService templateService, ILogger<EmailSender> logger)
    {
        _settings = settings;
        _templateService = templateService;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        try
        {
            _logger.LogInformation("Iniciando envio de email para {To}", message.To);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;

            _logger.LogDebug("Configurando corpo do email");
            var builder = new BodyBuilder();
            if (message.IsHtml)
            {
                builder.HtmlBody = message.Body;
                _logger.LogDebug("Corpo HTML configurado");
            }
            else
            {
                builder.TextBody = message.Body;
                _logger.LogDebug("Corpo texto configurado");
            }

            foreach (var attachment in message.Attachments)
            {
                _logger.LogDebug("Adicionando anexo: {Attachment}", attachment);
                builder.Attachments.Add(attachment);
            }

            email.Body = builder.ToMessageBody();

            _logger.LogInformation("Conectando ao servidor SMTP: {Server}:{Port}", _settings.SmtpServer, _settings.Port);
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, false);

            _logger.LogDebug("Autenticando com servidor SMTP");
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);

            _logger.LogInformation("Enviando email");
            await smtp.SendAsync(email);

            _logger.LogDebug("Desconectando do servidor SMTP");
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email enviado com sucesso para {To}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {To}: {Error}", message.To, ex.Message);
            throw;
        }
    }

    public async Task SendEmailWithTemplateAsync(string to, string templateName, Dictionary<string, string> variables)
    {
        var message = _templateService.ProcessTemplate(templateName, variables);
        message.To = to;
        await SendEmailAsync(message);
    }
}