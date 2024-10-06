using System.ComponentModel.DataAnnotations;

namespace foodswap.Options;

public class ConnectionStringsOptions
{
    public const string SectionName = "ConnectionStrings";
    [Required]
    public required string DefaultConnection { get; init; }
}