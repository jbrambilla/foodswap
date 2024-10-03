using System.ComponentModel.DataAnnotations;

namespace foodswap.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    [Required]
    public required string Secret { get; init; }
    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Audience { get; init; }
    [Required]
    [Range(30, 3000)]
    public required int ExpiryMinutes  { get; init; }
}