using System.ComponentModel.DataAnnotations;

namespace MailBot.Models;

/// <summary>
/// Representa um template de email com suporte a variáveis
/// </summary>
public class EmailTemplate
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    public bool IsHtml { get; set; }

    public Dictionary<string, string> Variables { get; set; } = new();

    /// <summary>
    /// Valida se todas as variáveis necessárias estão presentes
    /// </summary>
    public bool ValidateVariables(Dictionary<string, string> providedVariables)
    {
        foreach (var variable in Variables.Keys)
        {
            if (!providedVariables.ContainsKey(variable))
                return false;
        }
        return true;
    }
}