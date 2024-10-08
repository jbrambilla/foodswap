using foodswap.Common.Models;

namespace foodswap.Features.SwapperFeatures.Models;

public class FoodSwap : BaseModel
{
    public FoodSwap(Guid swapperId,string name, EFoodCategory category, decimal caloriesPerGram, decimal carbohydratesPerGram, decimal proteinPerGram, decimal fatPerGram)
    {
        SwapperId = swapperId;
        Name = name;
        Category = category;
        CaloriesPerGram = caloriesPerGram;
        CarbohydratesPerGram = carbohydratesPerGram;
        ProteinPerGram = proteinPerGram;
        FatPerGram = fatPerGram;
        IsMain = false;
    }
    public string Name { get; private set; } = string.Empty;
    public EFoodCategory Category { get; private set; }
    public decimal CaloriesPerGram { get; private set; }
    public decimal CarbohydratesPerGram { get; private set; }
    public decimal ProteinPerGram { get; private set; }
    public decimal FatPerGram { get; private set; }
    public bool IsMain { get; private set; }

    //EF RELATION
    public Guid SwapperId { get; private set; }
    public Swapper Swapper { get; private set; } = null!;

    public void DefineAsMain()
    {
        IsMain = true;
    }
}