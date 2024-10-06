using FluentValidation;
using foodswap.DTOs.UserDTOs;

namespace foodswap.Validators.UserEndpointsValidators;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}