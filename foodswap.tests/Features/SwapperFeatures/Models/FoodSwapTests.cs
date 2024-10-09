using foodswap.Features;
using foodswap.Features.SwapperFeatures.Models;

namespace foodswap.tests.Features.SwapperFeatures.Models;

public class FoodSwapTests
{
    [Fact]
    public void New_Should_Set_Macros_And_NotBeMain()
    {
        //Arrange
        int servingSize = 100;
        var caloriesPerGram = 0.9M;
        var carbohydratesPerGram = 0.22M;
        var proteinPerGram = 0.09M;
        var fatPerGram = 0.03M;

        //Act
        var foodSwap = new FoodSwap(new Guid(), "Banana", EFoodCategory.FRUITS, servingSize, caloriesPerGram, carbohydratesPerGram, proteinPerGram, fatPerGram);

        //Assert
        Assert.Equal(caloriesPerGram * servingSize, foodSwap.Calories);
        Assert.Equal(carbohydratesPerGram * servingSize, foodSwap.Carbohydrates);
        Assert.Equal(proteinPerGram * servingSize, foodSwap.Protein);
        Assert.Equal(fatPerGram * servingSize, foodSwap.Fat);
        Assert.False(foodSwap.IsMain);
    }

    [Fact]
    public void Update_ServingSize_Should_Set_Macros()
    {
        //Arrange
        int servingSizeToUpdate = 200;
        int servingSize = 100;
        var caloriesPerGram = 0.9M;
        var carbohydratesPerGram = 0.22M;
        var proteinPerGram = 0.09M;
        var fatPerGram = 0.03M;
        var foodSwap = new FoodSwap(new Guid(), "Banana", EFoodCategory.FRUITS, servingSize, caloriesPerGram, carbohydratesPerGram, proteinPerGram, fatPerGram);

        //Act
        foodSwap.UpdateServingSize(servingSizeToUpdate);

        //Assert
        Assert.Equal(caloriesPerGram * servingSizeToUpdate, foodSwap.Calories);
        Assert.Equal(carbohydratesPerGram * servingSizeToUpdate, foodSwap.Carbohydrates);
        Assert.Equal(proteinPerGram * servingSizeToUpdate, foodSwap.Protein);
        Assert.Equal(fatPerGram * servingSizeToUpdate, foodSwap.Fat);
    }

}