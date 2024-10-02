namespace foodswap.DTOs.FoodDTOs;
public class FoodResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Portion { get; set; }
    public decimal Calories { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public string Type { get; set; } = string.Empty;
}