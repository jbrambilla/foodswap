namespace foodswap.Features.SwapperFeatures.DTOs;

public class CreateFoodSwapRequest
{
    public string Name { get; set; } = string.Empty;
    public EFoodCategory Category { get; set; }
    public int ServingSize { get; set; }
    public decimal CaloriesPerGram { get; set; }
    public decimal CarbohydratesPerGram { get; set; }
    public decimal ProteinPerGram { get; set; }
    public decimal FatPerGram { get; set; }
}