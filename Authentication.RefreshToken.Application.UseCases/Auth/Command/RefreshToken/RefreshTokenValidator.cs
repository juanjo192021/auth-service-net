using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Auth.Command.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("The access token field is required.")
                .NotNull().WithMessage("The access token field cannot be null.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("The refresh token field is required.")
                .NotNull().WithMessage("The refresh token field cannot be null.");
        }
    }
}
