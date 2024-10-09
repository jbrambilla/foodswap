using foodswap.Common.Models;

namespace foodswap.Features.SwapperFeatures.Models;

public class FoodSwap : BaseModel
{
    public FoodSwap(Guid swapperId, string name, EFoodCategory category, int servingSize, decimal calories, decimal carbohydrates, decimal protein, decimal fat)
    {
        SwapperId = swapperId;
        Name = name;
        Category = category;
        ServingSize = servingSize;
        Calories = calories;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Fat = fat;
        IsMain = false;

        CaloriesPerGram = calories / servingSize;
        CarbohydratesPerGram = carbohydrates / servingSize;
        ProteinPerGram = protein / servingSize;
        FatPerGram = fat / servingSize;
    }
    public string Name { get; private set; } = string.Empty;
    public EFoodCategory Category { get; private set; }
    public int ServingSize { get; private set; }
    public decimal Calories { get; private set; }
    public decimal Carbohydrates { get; private set; }
    public decimal Protein { get; private set; }
    public decimal Fat { get; private set; }
    public decimal CaloriesPerGram { get; private set; }
    public decimal CarbohydratesPerGram { get; private set; }
    public decimal ProteinPerGram { get; private set; }
    public decimal FatPerGram { get; private set; }
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