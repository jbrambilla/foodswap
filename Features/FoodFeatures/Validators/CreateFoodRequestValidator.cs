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
        RuleFor(x => x.Category).Must(BeAValidEnumValue);
    }

     private bool BeAValidEnumValue(EFoodCategory category)
    {
        // Converte para o enum e verifica se é válido
        return Enum.IsDefined(typeof(EFoodCategory), category) && !category.Equals(default(EFoodCategory));
    }
}