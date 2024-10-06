using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace foodswap.Features.UserFeatures.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}