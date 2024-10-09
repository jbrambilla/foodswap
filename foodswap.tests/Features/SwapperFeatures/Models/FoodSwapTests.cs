using foodswap.Features;
using foodswap.Features.SwapperFeatures.Models;

namespace foodswap.tests.Features.SwapperFeatures.Models;

public class FoodSwapTests
{
    [Fact]
    public void New_Should_Set_Macros_And_NotBeMain()
    {
        //Arrange
        int servingSizeToUpdate = 200;
        int servingSize = 100;
        var calories = 90M;
        var carbohydrates = 22M;
        var protein = 9M;
        var fat = 3M;

        //Act
        var foodSwap = new FoodSwap(new Guid(), "Banana", EFoodCategory.FRUITS, servingSize, calories, carbohydrates, protein, fat);
 
        //Assert
        Assert.Equal(calories / servingSize, foodSwap.CaloriesPerGram);
        Assert.Equal(carbohydrates / servingSize, foodSwap.CarbohydratesPerGram);
        Assert.Equal(protein / servingSize, foodSwap.ProteinPerGram);
        Assert.Equal(fat / servingSize, foodSwap.FatPerGram);
        Assert.False(foodSwap.IsMain);
    }

    [Fact]
    public void Update_ServingSize_Should_Set_Macros()
    {
        //Arrange
        int servingSizeToUpdate = 200;
        int servingSize = 100;
        var calories = 90M;
        var carbohydrates = 22M;
        var protein = 9M;
        var fat = 3M;
        var foodSwap = new FoodSwap(new Guid(), "Banana", EFoodCategory.FRUITS, servingSize, calories, carbohydrates, protein, fat);

        //Act
        foodSwap.UpdateServingSize(servingSizeToUpdate);

        //Assert
        Assert.Equal(foodSwap.CaloriesPerGram * servingSizeToUpdate, foodSwap.Calories);
        Assert.Equal(foodSwap.CarbohydratesPerGram * servingSizeToUpdate, foodSwap.Carbohydrates);
        Assert.Equal(foodSwap.ProteinPerGram * servingSizeToUpdate, foodSwap.Protein);
        Assert.Equal(foodSwap.FatPerGram * servingSizeToUpdate, foodSwap.Fat);
    }

}