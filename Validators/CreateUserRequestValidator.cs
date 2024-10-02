using FluentValidation;

namespace foodswap.DTOs.UserDTOs;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(100);

        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Length(11)
            .WithMessage("Phone number must be exactly 11 digits.")
            .Matches(@"^\d+$")
            .WithMessage("Phone number must contain only numbers.");

        RuleFor(x => x.Password).NotEmpty();

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .Must(BeAtLeast12YearsOld)
            .WithMessage("User must be at least 12 years old.");
    }

    private bool BeAtLeast12YearsOld(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        // Se ainda não fez aniversário este ano, subtrai 1 da idade
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age >= 12;
    }
}