namespace foodswap.Features.SwapperFeatures.DTOs;

public class CreateFoodSwapRequest
{
    public string Name { get; set; } = string.Empty;
    public int ServingSize { get; set; }
    public decimal Calories { get; set; }
    public decimal Carbohydrates{ get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public EFoodCategory Category { get; set; }
}