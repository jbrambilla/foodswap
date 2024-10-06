using System.ComponentModel.DataAnnotations;

namespace foodswap.Options;

public class SentrySettingsOptions
{
    public const string SectionName = "SentrySettings";
    [Required]
    public required string Dsn { get; init; }
}