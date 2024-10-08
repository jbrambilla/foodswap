using foodswap.Features;
using foodswap.Features.FoodFeatures;

namespace foodswap.tests.Features.FoodFeatures;

public class FoodTests
{
    [Fact]
    public void New_Food_Should_Set_PerGrams_And_BeActive()
    {
        //Arrange
        int servingSize = 100;
        var calories = 90M;
        var carbohydrates = 21.8M;
        var protein = 9M;
        var fat = 3M;

        //Act
        var food = new Food("Banana", servingSize, calories, carbohydrates, protein, fat, EFoodCategory.FRUITS);

        //Assert
        Assert.Equal(calories / servingSize, food.CaloriesPerGram);
        Assert.Equal(carbohydrates / servingSize, food.CarbohydratesPerGram);
        Assert.Equal(protein / servingSize, food.ProteinPerGram);
        Assert.Equal(fat / servingSize, food.FatPerGram);
        Assert.True(food.IsActive);
    }

    [Fact]
    public void Update_Food_Should_Set_PerGrams_And_Keep_IsActive_State()
    {
        //Arrange
        var servingSize = 100;
        var calories = 90M;
        var carbohydrates = 21.8M;
        var protein = 9M;
        var fat = 3M;

        var food = new Food("Banana", servingSize, calories, carbohydrates, protein, fat, EFoodCategory.FRUITS);

        var servingSizeUpdated = 200;
        var caloriesUpdated = 180M;
        var carbohydratesUpdated = 43.6M;
        var proteinUpdated = 18M;
        var fatUpdated = 6M;

        //Act
        food.Update("Banana", servingSizeUpdated, EFoodCategory.FRUITS, caloriesUpdated, carbohydratesUpdated, proteinUpdated, fatUpdated);

        //Assert

        //Garantir que valores antigos foram alterados
        Assert.NotEqual(food.Calories, calories);
        Assert.NotEqual(food.Carbohydrates, carbohydrates);
        Assert.NotEqual(food.Protein, protein);
        Assert.NotEqual(food.Fat, fat);

        //Garantir que atualizou e setou corretamente os valores pergram
        Assert.Equal(food.Calories, caloriesUpdated);
        Assert.Equal(food.Carbohydrates, carbohydratesUpdated);
        Assert.Equal(food.Protein, proteinUpdated);
        Assert.Equal(food.Fat, fatUpdated);

        Assert.Equal(food.CaloriesPerGram, caloriesUpdated / servingSizeUpdated);
        Assert.Equal(food.CarbohydratesPerGram, carbohydratesUpdated / servingSizeUpdated);
        Assert.Equal(food.ProteinPerGram, proteinUpdated / servingSizeUpdated);
        Assert.Equal(food.FatPerGram, fatUpdated / servingSizeUpdated);

        Assert.True(food.IsActive);
    }

    [Fact]
    public void ActivateFood_Should_ActiveFood()
    {
        //Arrange
        var food = new Food("Banana", 100, 90, 21.8M, 9, 3, EFoodCategory.FRUITS);

        //Act
        food.Activate();

        //Assert
        Assert.True(food.IsActive);
    }

    [Fact]
    public void DeactivateFood_Should_DeactiveFood()
    {
        //Arrange
        var food = new Food("Banana", 100, 90, 21.8M, 9, 3, EFoodCategory.FRUITS);

        //Act
        food.Deactivate();

        //Assert
        Assert.False(food.IsActive);
    }
}