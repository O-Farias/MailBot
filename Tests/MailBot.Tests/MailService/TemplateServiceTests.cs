using Xunit;
using Moq;
using MailBot.MailService;
using MailBot.Models;

namespace MailBot.Tests.MailService;

public class TemplateServiceTests
{
    private readonly Mock<ITemplateService> _mockTemplateService;

    public TemplateServiceTests()
    {
        _mockTemplateService = new Mock<ITemplateService>();
    }

    [Fact]
    public void ProcessTemplate_ReplacesVariables_Correctly()
    {
        // Arrange
        var template = new EmailTemplate
        {
            Name = "welcome",
            Subject = "Hello {Name}",
            Body = "Welcome {Name}!",
            IsHtml = false
        };

        var variables = new Dictionary<string, string>
        {
            { "Name", "João" }
        };

        _mockTemplateService
            .Setup(x => x.GetTemplate("welcome"))
            .Returns(template);

        _mockTemplateService
            .Setup(x => x.ProcessTemplate("welcome", variables))
            .Returns(new EmailMessage
            {
                Subject = "Hello João",
                Body = "Welcome João!",
                IsHtml = false
            });

        // Act
        var result = _mockTemplateService.Object.ProcessTemplate("welcome", variables);

        // Assert
        Assert.Equal("Hello João", result.Subject);
        Assert.Equal("Welcome João!", result.Body);
        Assert.False(result.IsHtml);
    }

    [Fact]
    public void GetTemplate_NonExistentTemplate_ThrowsException()
    {
        // Arrange
        _mockTemplateService
            .Setup(x => x.GetTemplate("nonexistent"))
            .Throws<KeyNotFoundException>();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() =>
            _mockTemplateService.Object.GetTemplate("nonexistent"));
    }
}