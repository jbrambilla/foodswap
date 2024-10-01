using FluentValidation;
using foodswap.DTOs;

namespace foodswap.Validators;
public class FoodRequestValidator : AbstractValidator<FoodRequest>
{
    public FoodRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Calories).GreaterThan(0);
        RuleFor(x => x.Carbohydrates).GreaterThan(0);
        RuleFor(x => x.Protein).GreaterThan(0);
        RuleFor(x => x.Fat).GreaterThan(0);
    }
}