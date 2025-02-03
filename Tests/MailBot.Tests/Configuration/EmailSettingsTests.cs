using Xunit;
using Microsoft.Extensions.Configuration;
using MailBot.Models;

namespace MailBot.Tests.Configuration;

public class EmailSettingsTests
{
    [Fact]
    public void EmailSettings_LoadsFromConfiguration()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string> {
            {"EmailSettings:SmtpServer", "smtp.example.com"},
            {"EmailSettings:Port", "587"},
            {"EmailSettings:Username", "user@example.com"},
            {"EmailSettings:Password", "password"},
            {"EmailSettings:FromName", "MailBot"},
            {"EmailSettings:FromAddress", "noreply@example.com"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();

        // Assert
        Assert.NotNull(emailSettings);
        Assert.Equal("smtp.example.com", emailSettings.SmtpServer);
        Assert.Equal(587, emailSettings.Port);
        Assert.Equal("user@example.com", emailSettings.Username);
        Assert.Equal("password", emailSettings.Password);
        Assert.Equal("MailBot", emailSettings.FromName);
        Assert.Equal("noreply@example.com", emailSettings.FromAddress);
    }
}