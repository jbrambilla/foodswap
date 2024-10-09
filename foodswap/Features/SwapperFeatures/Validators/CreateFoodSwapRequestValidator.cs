using FluentValidation;
using foodswap.Features.SwapperFeatures.DTOs;

namespace foodswap.Features.SwapperFeatures.Validators;

public class CreateFoodSwapRequestValidator : AbstractValidator<CreateFoodSwapRequest>
{
    public CreateFoodSwapRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Category).Must(BeAValidEnumValue);
        RuleFor(x => x.ServingSize).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CaloriesPerGram).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CarbohydratesPerGram).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProteinPerGram).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FatPerGram).GreaterThanOrEqualTo(0);
    }

    private bool BeAValidEnumValue(EFoodCategory category)
    {
        // Converte para o enum e verifica se é válido
        return Enum.IsDefined(typeof(EFoodCategory), category) && !category.Equals(default(EFoodCategory));
    }
}