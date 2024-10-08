using foodswap.Common.Models;

namespace foodswap.Features.FoodFeatures;
public class Food : BaseModel
{
    public Food(string name, int servingSize, decimal calories, decimal carbohydrates, decimal protein, decimal fat, EFoodCategory category)
    {
        Name = name;
        ServingSize = servingSize;
        Category = category;
        Calories = calories;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Fat = fat;
        IsActive = true;

        CaloriesPerGram = calories / servingSize;
        CarbohydratesPerGram = carbohydrates / servingSize;
        ProteinPerGram = protein / servingSize;
        FatPerGram = fat / servingSize;
    }

    public string Name { get; private set; } = string.Empty;
    public int ServingSize { get; private set; }
    public EFoodCategory Category { get; private set; }
    public decimal Calories { get; private set; }
    public decimal Carbohydrates { get; private set; }
    public decimal Protein { get; private set; }
    public decimal Fat { get; private set; }
    public decimal CaloriesPerGram { get; private set; }
    public decimal CarbohydratesPerGram { get; private set; }
    public decimal ProteinPerGram { get; private set; }
    public decimal FatPerGram { get; private set; }
    public bool IsActive { get; private set; }

    public void Update(string name, int servingSize, EFoodCategory category, decimal calories, decimal carbohydrates, decimal protein, decimal fat)
    {
        Name = name;
        ServingSize = servingSize;
        Category = category;
        Calories = calories;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Fat = fat;

        CaloriesPerGram = calories / servingSize;
        CarbohydratesPerGram = carbohydrates / servingSize;
        ProteinPerGram = protein / servingSize;
        FatPerGram = fat / servingSize;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}