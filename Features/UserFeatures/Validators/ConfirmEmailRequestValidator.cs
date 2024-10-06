using FluentValidation;
using foodswap.Features.UserFeatures.UserDTOs;

namespace foodswap.Features.UserFeatures.Validators;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}