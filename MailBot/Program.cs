using MailBot.MailService;
using MailBot.Models;
using Microsoft.Extensions.Configuration;

// Configuração
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();
var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>()
    ?? throw new InvalidOperationException("Configurações de email não encontradas");

// Serviços
var templateService = new TemplateService();
var emailSender = new EmailSender(emailSettings, templateService);

// Templates
templateService.AddTemplate("welcome", new EmailTemplate
{
    Name = "welcome",
    Subject = "Bem-vindo, {UserName}!",
    Body = "<h1>Olá {UserName}!</h1><p>Bem-vindo ao nosso serviço.</p>",
    IsHtml = true
});

// Envio de email
try
{
    await emailSender.SendEmailWithTemplateAsync(
        "destinatario@example.com",
        "welcome",
        new Dictionary<string, string>
        {
            { "UserName", "João" }
        }
    );
    Console.WriteLine("Email enviado com sucesso!");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao enviar email: {ex.Message}");
    Environment.ExitCode = 1;
}