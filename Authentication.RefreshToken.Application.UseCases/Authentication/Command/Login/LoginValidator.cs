using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email field is required.")
                .NotNull().WithMessage("The email field cannot be null.");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field is required.")
                .MinimumLength(5).WithMessage("The password must be at least 5 characters long.")
                .MaximumLength(100).WithMessage("The password cannot exceed 100 characters.");
        }
    }
}
