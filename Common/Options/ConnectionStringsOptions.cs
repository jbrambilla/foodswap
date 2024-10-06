using System.ComponentModel.DataAnnotations;

namespace foodswap.Common.Options;

public class ConnectionStringsOptions
{
    public const string SectionName = "ConnectionStrings";
    [Required]
    public required string DefaultConnection { get; init; }
}