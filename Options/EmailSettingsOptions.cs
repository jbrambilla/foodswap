using System.ComponentModel.DataAnnotations;

namespace foodswap.Options;

public class EmailSettingsOptions
{
    public const string SectionName = "EmailSettings";

    [Required]
    public required string SmtpServer { get; init; }
    [Required]
    public required int Port { get; init; }
    [Required]
    public required string UserEmail { get; init; }
    [Required]
    public required string Password { get; init; }
    [Required]
    public required string SenderEmail { get; init; }
}