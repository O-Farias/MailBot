
using Xunit;
using MailBot.Models;
using System.ComponentModel.DataAnnotations;

namespace MailBot.Tests.Models;

public class EmailMessageTests
{
    [Fact]
    public void EmailMessage_InvalidEmail_ThrowsValidationException()
    {
        // Arrange
        var message = new EmailMessage
        {
            To = "invalid-email",
            Subject = "Test",
            Body = "Test body"
        };

        // Act & Assert
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(message);
        var isValid = Validator.TryValidateObject(message, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(EmailMessage.To)));
    }
}