
using Xunit;
using MailBot.MailService;
using MailBot.Models;

namespace MailBot.Tests.MailService;

public class TemplateServiceTests
{
    [Fact]
    public void ProcessTemplate_ReplacesVariables_Correctly()
    {
        // Arrange
        var service = new TemplateService();
        var template = new EmailTemplate
        {
            Name = "welcome",
            Subject = "Hello {Name}",
            Body = "Welcome {Name}!",
            IsHtml = false
        };

        service.AddTemplate("welcome", template);

        var variables = new Dictionary<string, string>
        {
            { "Name", "João" }
        };

        // Act
        var result = service.ProcessTemplate("welcome", variables);

        // Assert
        Assert.Equal("Hello João", result.Subject);
        Assert.Equal("Welcome João!", result.Body);
        Assert.False(result.IsHtml);
    }

    [Fact]
    public void GetTemplate_NonExistentTemplate_ThrowsException()
    {
        // Arrange
        var service = new TemplateService();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => service.GetTemplate("nonexistent"));
    }
}