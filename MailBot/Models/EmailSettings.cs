using System.ComponentModel.DataAnnotations;

namespace MailBot.Models;

public record EmailSettings
{
    [Required]
    public string SmtpServer { get; init; } = string.Empty;

    [Required]
    [Range(1, 65535)]
    public int Port { get; init; }

    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    [Required]
    public string FromName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string FromAddress { get; init; } = string.Empty;
}