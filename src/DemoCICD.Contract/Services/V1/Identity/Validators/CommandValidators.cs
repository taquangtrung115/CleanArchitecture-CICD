using FluentValidation;

namespace DemoCICD.Contract.Services.V1.Identity.Validators;

public class LogoutValidator : AbstractValidator<Command.Logout>
{
    public LogoutValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required");
    }
}

public class RefreshTokenValidator : AbstractValidator<Command.RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}