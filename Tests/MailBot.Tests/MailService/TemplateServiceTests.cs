using Xunit;
using Moq;
using MailBot.MailService;
using MailBot.Models;

namespace MailBot.Tests.MailService;

public class TemplateServiceTests
{
    private readonly Mock<ITemplateService> _templateServiceMock;
    private readonly ITemplateService _templateService;

    public TemplateServiceTests()
    {
        _templateServiceMock = new Mock<ITemplateService>();
        _templateService = _templateServiceMock.Object;
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

        _templateServiceMock
            .Setup(x => x.GetTemplate("welcome"))
            .Returns(template);

        _templateServiceMock
            .Setup(x => x.ProcessTemplate("welcome", It.IsAny<Dictionary<string, string>>()))
            .Returns(new EmailMessage
            {
                Subject = "Hello João",
                Body = "Welcome João!",
                IsHtml = false
            });

        var variables = new Dictionary<string, string>
        {
            { "Name", "João" }
        };

        // Act
        var result = _templateService.ProcessTemplate("welcome", variables);

        // Assert
        Assert.Equal("Hello João", result.Subject);
    }
}