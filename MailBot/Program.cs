using MailBot.MailService;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();

// Configura as definições de email
var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
var emailSender = new EmailSender(emailSettings!);

// Exemplo de uso
var message = new EmailMessage
{
    To = "destinatario@example.com",
    Subject = "Teste do MailBot",
    Body = "Este é um teste do serviço de envio de emails.",
    IsHtml = false
};

try
{
    await emailSender.SendEmailAsync(message);
    Console.WriteLine("Email enviado com sucesso!");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao enviar email: {ex.Message}");
}