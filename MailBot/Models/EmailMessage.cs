using System.ComponentModel.DataAnnotations;

public class EmailMessage
{
    [Required]
    [EmailAddress]
    public string To { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    public bool IsHtml { get; set; }

    public List<string> Attachments { get; set; } = new List<string>();
}