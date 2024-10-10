namespace foodswap.Features.SwapperFeatures.DTOs;
public class GetSuggestionResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public List<FoodSuggestionResponse> Foods { get; set; } = [];
}
public class FoodSuggestionResponse
{
    public Guid Id { get; set; }
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
    public string Category { get; set; } = string.Empty;
}