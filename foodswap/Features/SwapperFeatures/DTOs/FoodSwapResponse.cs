namespace foodswap.Features.SwapperFeatures.DTOs;

public class FoodSwapResponse
{
    public Guid Id { get; set; }
    public Guid SwapperId { get; set; }
    public string Name { get; set; } = string.Empty;
    public EFoodCategory Category { get; set; }
    public decimal CaloriesPerGram { get; set; }
    public decimal CarbohydratesPerGram { get; set; }
    public decimal ProteinPerGram { get; set; }
    public decimal FatPerGram { get; set; }
    public bool IsMain { get; set; }
}