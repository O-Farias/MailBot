
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MailBot.MailService;
using MailBot.Models;

namespace MailBot.Tests.MailService;

public class EmailSenderTests
{
    private readonly Mock<ILogger<EmailSender>> _loggerMock;
    private readonly Mock<TemplateService> _templateServiceMock;
    private readonly EmailSettings _settings;

    public EmailSenderTests()
    {
        _loggerMock = new Mock<ILogger<EmailSender>>();
        _templateServiceMock = new Mock<TemplateService>();
        _settings = new EmailSettings
        {
            SmtpServer = "smtp.test.com",
            Port = 587,
            Username = "test@test.com",
            Password = "password",
            FromName = "Test",
            FromAddress = "test@test.com"
        };
    }

    [Fact]
    public async Task SendEmailWithTemplate_ValidTemplate_CallsSendEmail()
    {
        // Arrange
        var message = new EmailMessage
        {
            To = "test@test.com",
            Subject = "Test",
            Body = "Test body",
            IsHtml = false
        };

        _templateServiceMock
            .Setup(x => x.ProcessTemplate(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Returns(message);

        var sender = new EmailSender(_settings, _templateServiceMock.Object, _loggerMock.Object);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() =>
            sender.SendEmailWithTemplateAsync("test@test.com", "test", new Dictionary<string, string>()));

        Assert.Null(exception);
    }
}