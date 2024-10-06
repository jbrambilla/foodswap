using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace foodswap.Validators.UserEndpointsValidators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}