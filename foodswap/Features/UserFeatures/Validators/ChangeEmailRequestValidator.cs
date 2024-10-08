using FluentValidation;
using foodswap.Features.UserFeatures.UserDTOs;

namespace foodswap.Features.UserFeatures.Validators;

public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
{
    public ChangeEmailRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty();
    }
}