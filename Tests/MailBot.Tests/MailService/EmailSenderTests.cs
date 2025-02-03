using Xunit;
using Moq;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MailBot.MailService;
using MailBot.Models;

namespace MailBot.Tests.MailService;

public class EmailSenderTests
{
    private readonly Mock<ILogger<EmailSender>> _loggerMock;
    private readonly Mock<ITemplateService> _templateServiceMock;
    private readonly EmailSettings _settings;
    private readonly EmailSender _sut;

    public EmailSenderTests()
    {
        _loggerMock = new Mock<ILogger<EmailSender>>();
        _templateServiceMock = new Mock<ITemplateService>();
        _settings = new EmailSettings
        {
            SmtpServer = "smtp.test.com",
            Port = 587,
            Username = "test@test.com",
            Password = "password",
            FromName = "Test",
            FromAddress = "test@test.com"
        };

        _sut = new EmailSender(_settings, _templateServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task SendEmailAsync_ValidMessage_SendsEmail()
    {
        // Arrange
        var message = new EmailMessage
        {
            To = "test@test.com",
            Subject = "Test",
            Body = "Test body",
            IsHtml = false
        };

        // Act
        await _sut.SendEmailAsync(message);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Iniciando envio de email")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
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
        _templateServiceMock.Verify(x => x.ProcessTemplate(
            It.IsAny<string>(),
            It.IsAny<Dictionary<string, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SendEmailWithTemplate_InvalidTemplate_ThrowsException()
    {
        // Arrange
        _templateServiceMock
            .Setup(x => x.ProcessTemplate(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Throws<KeyNotFoundException>();

        var sender = new EmailSender(_settings, _templateServiceMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            sender.SendEmailWithTemplateAsync("test@test.com", "nonexistent", new Dictionary<string, string>()));
    }
}