using MailBot.MailService;
using MailBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/mailbot.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    // Configuração
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    var configuration = builder.Build();
    var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>()
        ?? throw new InvalidOperationException("Configurações de email não encontradas");

    // Configuração do logging
    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddSerilog(dispose: true)
            .SetMinimumLevel(LogLevel.Debug);
    });

    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogInformation("Iniciando MailBot");

    // Serviços
    var templateService = new TemplateService();
    var emailLogger = loggerFactory.CreateLogger<EmailSender>();
    var emailSender = new EmailSender(emailSettings, templateService, emailLogger);

    // Templates
    templateService.AddTemplate("welcome", new EmailTemplate
    {
        Name = "welcome",
        Subject = "Bem-vindo, {UserName}!",
        Body = "<h1>Olá {UserName}!</h1><p>Bem-vindo ao nosso serviço.</p>",
        IsHtml = true
    });

    logger.LogInformation("Enviando email de teste");

    // Envio de email
    await emailSender.SendEmailWithTemplateAsync(
        "destinatario@example.com",
        "welcome",
        new Dictionary<string, string>
        {
            { "UserName", "João" }
        }
    );

    logger.LogInformation("Email enviado com sucesso");
}
catch (Exception ex)
{
    Log.Error(ex, "Erro na execução do programa");
    Environment.ExitCode = 1;
}
finally
{
    Log.CloseAndFlush();
}