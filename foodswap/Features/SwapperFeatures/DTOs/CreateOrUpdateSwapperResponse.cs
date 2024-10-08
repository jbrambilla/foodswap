namespace foodswap.Features.SwapperFeatures.DTOs;

public class CreateOrUpdateSwapperResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}