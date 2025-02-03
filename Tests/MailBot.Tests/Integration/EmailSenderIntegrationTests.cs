
using Xunit;
using MailBot.MailService;
using MailBot.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace MailBot.Tests.Integration;

public class EmailSenderIntegrationTests
{
    private readonly EmailSettings _settings;
    private readonly Mock<ILogger<EmailSender>> _loggerMock;
    private readonly TemplateService _templateService;
    private readonly EmailSender _emailSender;

    public EmailSenderIntegrationTests()
    {
        _settings = new EmailSettings
        {
            SmtpServer = "smtp.test.com",
            Port = 587,
            Username = "test@test.com",
            Password = "password",
            FromName = "Test",
            FromAddress = "test@test.com"
        };

        _loggerMock = new Mock<ILogger<EmailSender>>();
        _templateService = new TemplateService();
        _emailSender = new EmailSender(_settings, _templateService, _loggerMock.Object);
    }

    [Fact]
    public async Task SendEmailAsync_ValidMessage_SendsEmail()
    {
        // Arrange
        var message = new EmailMessage
        {
            To = "recipient@test.com",
            Subject = "Email Teste",
            Body = "Esse é um email de teste.",
            IsHtml = false
        };

        // Act
        var exception = await Record.ExceptionAsync(() => _emailSender.SendEmailAsync(message));

        // Assert
        Assert.Null(exception);
    }
}