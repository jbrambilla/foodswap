using FluentValidation;
using foodswap.Features.FoodFeatures.FoodDTOs;

namespace foodswap.Features.FoodFeatures.Validators;
public class CreateFoodRequestValidator : AbstractValidator<CreateOrUpdateFoodRequest>
{
    public CreateFoodRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ServingSize).GreaterThan(0);
        RuleFor(x => x.Calories).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Carbohydrates).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Protein).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Fat).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Category).Must(BeAValidEnumValue);
    }

     private bool BeAValidEnumValue(EFoodCategory category)
    {
        // Converte para o enum e verifica se é válido
        return Enum.IsDefined(typeof(EFoodCategory), category) && !category.Equals(default(EFoodCategory));
    }
}