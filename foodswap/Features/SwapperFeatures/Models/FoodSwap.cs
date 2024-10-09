using foodswap.Common.Models;

namespace foodswap.Features.SwapperFeatures.Models;

public class FoodSwap : BaseModel
{
    public FoodSwap(Guid swapperId,string name, EFoodCategory category, int servingSize, decimal caloriesPerGram, decimal carbohydratesPerGram, decimal proteinPerGram, decimal fatPerGram)
    {
        SwapperId = swapperId;
        Name = name;
        Category = category;
        ServingSize = servingSize;
        CaloriesPerGram = caloriesPerGram;
        CarbohydratesPerGram = carbohydratesPerGram;
        ProteinPerGram = proteinPerGram;
        FatPerGram = fatPerGram;
        IsMain = false;

        Calories = caloriesPerGram * servingSize;
        Carbohydrates = carbohydratesPerGram * servingSize;
        Protein = proteinPerGram * servingSize;
        Fat = fatPerGram * servingSize;
    }
    public string Name { get; private set; } = string.Empty;
    public EFoodCategory Category { get; private set; }
    public int ServingSize { get; private set; }
    public decimal CaloriesPerGram { get; private set; }
    public decimal CarbohydratesPerGram { get; private set; }
    public decimal ProteinPerGram { get; private set; }
    public decimal FatPerGram { get; private set; }

    public decimal Calories { get; private set; }
    public decimal Carbohydrates { get; private set; }
    public decimal Protein { get; private set; }
    public decimal Fat { get; private set; }

    public bool IsMain { get; private set; }

    //EF RELATION
    public Guid SwapperId { get; private set; }
    public Swapper? Swapper { get; private set; }

    public void DefineAsMain()
    {
        IsMain = true;
    }

    public void UnDefineAsMain()
    {
        IsMain = false;
    }

    public void UpdateServingSize(int servingSize)
    {
        ServingSize = servingSize;

        Calories = CaloriesPerGram * servingSize;
        Carbohydrates = CarbohydratesPerGram * servingSize;
        Protein = ProteinPerGram * servingSize;
        Fat = FatPerGram * servingSize;
    }
}