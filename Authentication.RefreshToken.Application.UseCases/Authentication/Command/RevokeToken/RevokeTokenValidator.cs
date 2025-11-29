using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.RevokeToken
{
    public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("The RefresherToken must not be empty.")
                .NotNull().WithMessage("The RefresherToken must not be null.");
        }
    }
}
