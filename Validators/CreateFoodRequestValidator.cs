using FluentValidation;
using foodswap.Business.Models;
using foodswap.DTOs.FoodDTOs;

namespace foodswap.Validators;
public class CreateFoodRequestValidator : AbstractValidator<CreateFoodRequest>
{
    public CreateFoodRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Portion).GreaterThan(0);
        RuleFor(x => x.Calories).GreaterThan(0);
        RuleFor(x => x.Carbohydrates).GreaterThan(0);
        RuleFor(x => x.Protein).GreaterThan(0);
        RuleFor(x => x.Fat).GreaterThan(0);
        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(BeValidFoodType)
            .WithMessage($"The type mu be one of the following: {string.Join(", ", FoodTypes.GetTypes())}.");
    }

    private bool BeValidFoodType(string type)
    {
        return FoodTypes.GetTypes().Contains(type);
    }
}