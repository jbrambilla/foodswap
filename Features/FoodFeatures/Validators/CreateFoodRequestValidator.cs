using FluentValidation;
using foodswap.Features.FoodFeatures.FoodDTOs;

namespace foodswap.Features.FoodFeatures.Validators;
public class CreateFoodRequestValidator : AbstractValidator<CreateFoodRequest>
{
    public CreateFoodRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ServingSize).GreaterThan(0);
        RuleFor(x => x.Calories).GreaterThan(0);
        RuleFor(x => x.Carbohydrates).GreaterThan(0);
        RuleFor(x => x.Protein).GreaterThan(0);
        RuleFor(x => x.Fat).GreaterThan(0);
        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(BeValidFoodType)
            .WithMessage($"The type mu be one of the following: {string.Join(", ", FoodCategories.GetCategories())}.");
    }

    private bool BeValidFoodType(string type)
    {
        return FoodCategories.GetCategories().Contains(type);
    }
}