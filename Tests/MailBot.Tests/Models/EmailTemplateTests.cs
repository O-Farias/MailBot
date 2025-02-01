
using Xunit;
using MailBot.Models;

namespace MailBot.Tests.Models;

public class EmailTemplateTests
{
    [Fact]
    public void ValidateVariables_WithValidVariables_ReturnsTrue()
    {
        // Arrange
        var template = new EmailTemplate
        {
            Name = "test",
            Subject = "Hello {Name}",
            Body = "Welcome {Name}!",
            Variables = new Dictionary<string, string>
            {
                { "Name", "Nome do usuário" }
            }
        };

        var variables = new Dictionary<string, string>
        {
            { "Name", "João" }
        };

        // Act
        var result = template.ValidateVariables(variables);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateVariables_WithMissingVariables_ReturnsFalse()
    {
        // Arrange
        var template = new EmailTemplate
        {
            Variables = new Dictionary<string, string>
            {
                { "Name", "Nome do usuário" }
            }
        };

        var variables = new Dictionary<string, string>();

        // Act
        var result = template.ValidateVariables(variables);

        // Assert
        Assert.False(result);
    }
}