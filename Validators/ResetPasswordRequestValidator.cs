using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace foodswap.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.ResetCode).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}