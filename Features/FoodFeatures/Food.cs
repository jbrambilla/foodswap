using foodswap.Common.Models;

namespace foodswap.Features.FoodFeatures;
public class Food : BaseModel
{
    public Food(string name, int servingSize, decimal calories, decimal carbohydrates, decimal protein, decimal fat, string type)
    {
        Name = name;
        ServingSize = servingSize;
        Type = type;
        Calories = calories;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Fat = fat;
        Active = true;

        CaloriesPerGram = calories / servingSize;
        CarbohydratesPerGram = carbohydrates / servingSize;
        ProteinPerGram = protein / servingSize;
        FatPerGram = fat / servingSize;
    }
    public string Name { get; private set; } = string.Empty;
    public int ServingSize { get; private set; }
    public string Type { get; private set; }
    public decimal Calories { get; private set; }
    public decimal Carbohydrates { get; private set; }
    public decimal Protein { get; private set; }
    public decimal Fat { get; private set; }

    public decimal CaloriesPerGram { get; private set; }
    public decimal CarbohydratesPerGram { get; private set; }
    public decimal ProteinPerGram { get; private set; }
    public decimal FatPerGram { get; private set; }

    public bool Active { get; private set; }

    public void Update(string name, int servingSize, string type, decimal calories, decimal carbohydrates, decimal protein, decimal fat)
    {
        Name = name;
        ServingSize = servingSize;
        Type = type;
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
        Active = false;
    }

    public void Activate()
    {
        Active = true;
    }
}

public static class FoodTypes 
{
    public const string VEGETABLE = "VEGETABLE";
    public const string FRUIT = "FRUIT";
    public const string MEAT = "MEAT";
    public const string DAIRY = "DAIRY";
    public const string GRAIN = "GRAIN";
    public const string OTHER = "OTHER";

    public static List<string> GetTypes() => new List<string> { VEGETABLE, FRUIT, MEAT, DAIRY, GRAIN, OTHER };
}