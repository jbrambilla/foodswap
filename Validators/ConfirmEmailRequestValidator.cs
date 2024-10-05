using FluentValidation;
using foodswap.DTOs.UserDTOs;

namespace foodswap.Validators;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ConfirmationToken).NotEmpty();
    }
}