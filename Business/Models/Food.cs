public class Food : BaseModel
{
    public Food(string name, decimal calories, decimal carbohydrates, decimal protein, decimal fat)
    {
        Name = name;
        Calories = calories;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Fat = fat;
        Active = true;
    }
    public string Name { get; private set; } = string.Empty;
    public decimal Calories { get; private set; }
    public decimal Carbohydrates { get; private set; }
    public decimal Protein { get; private set; }
    public decimal Fat { get; private set; }
    public bool Active { get; private set; }

    public void Update(string name, decimal calories, decimal carbohydrates, decimal protein, decimal fat)
    {
        Name = name;
        Calories = calories;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Fat = fat;
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