using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email field is required.")
                .NotNull().WithMessage("The email field cannot be null.")
                .EmailAddress().WithMessage("Debe ser un correo válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field is required.")
                .NotNull().WithMessage("The password field cannot be null.")
                .MinimumLength(4).WithMessage("The password field must have at least 4 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The first name field is required.")
                .NotNull().WithMessage("The first name field cannot be null.")
                .MinimumLength(2).WithMessage("The first name field must have at least 2 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("The last name field is required.")
                .NotNull().WithMessage("The last name field cannot be null.")
                .MinimumLength(2).WithMessage("The last name field must have at least 2 characters");
        }
    }
}
