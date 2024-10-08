namespace foodswap.Features.FoodFeatures.FoodDTOs;

public class GetAllFoodRequest
{ 
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? Sort { get; set; }
}