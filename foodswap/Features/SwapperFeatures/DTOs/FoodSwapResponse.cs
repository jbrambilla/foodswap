namespace foodswap.Features.SwapperFeatures.DTOs;

public class FoodSwapResponse
{
    public Guid Id { get; set; }
    public Guid SwapperId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ServingSize { get; set; }
    public decimal Calories { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public decimal CaloriesPerGram { get; set; }
    public decimal CarbohydratesPerGram { get; set; }
    public decimal ProteinPerGram { get; set; }
    public decimal FatPerGram { get; set; }
    public EFoodCategory Category { get; set; }
    public bool IsMain { get; set; }
}