namespace foodswap.DTOs;
public class FoodRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Calories { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
}