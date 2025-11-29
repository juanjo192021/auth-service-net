using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh
{
    public class RefreshValidator : AbstractValidator<RefreshCommand>
    {
        public RefreshValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("The refresh token field is required.")
                .NotNull().WithMessage("The refresh token field cannot be null.");
        }
    }
}
