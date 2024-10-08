namespace foodswap.Features.FoodFeatures.FoodDTOs;

public class GetAllFoodResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public List<FoodResumedResponse> Foods { get; set; } = [];
}