namespace foodswap.Features.SwapperFeatures.DTOs;

public class GetSwapperByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<FoodSwapResponse> FoodSwaps { get; set; } = new List<FoodSwapResponse>();
}