using System.ComponentModel.DataAnnotations;

namespace foodswap.Options;

public class IdentitySeedOptions
{
    public const string SectionName = "IdentitySeed";
    [Required]
    public required List<string> Roles { get; init; }
    [Required]
    public required AdminUser AdminUser { get; init; }
}

public class AdminUser
{
    [Required]
    public required string Name { get; init; }
    [Required]
    public required string Email { get; init; }
    [Required]
    [MinLength(8)]
    public required string Password { get; init; }
    [Required]
    public required string Role { get; init; }
}