using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .NotNull().WithMessage("First name cannot be null.")
                .MaximumLength(30).WithMessage("First name max length is 30.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .NotNull().WithMessage("Last name cannot be null.")
                .MaximumLength(30).WithMessage("Last name max length is 30.");

            RuleFor(x => x.DocumentType)
                .MaximumLength(20)
                .When(x => !string.IsNullOrEmpty(x.DocumentType))
                .WithMessage("Document type max length is 20.");

            RuleFor(x => x.DocumentNumber)
                .MaximumLength(20)
                .When(x => !string.IsNullOrEmpty(x.DocumentNumber))
                .WithMessage("Document number max length is 20.");

            RuleFor(x => x.Phone)
                .Matches(@"^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Phone must contain only digits.");

            RuleFor(x => x.Mobile)
                .Matches(@"^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Mobile))
                .WithMessage("Mobile must contain only digits.");

            RuleFor(x => x.Gender)
                .Must(x => string.IsNullOrEmpty(x) || new[] { "M", "F", "O" }.Contains(x))
                .WithMessage("Gender must be M, F, or O.")
                .When(x => !string.IsNullOrEmpty(x.Gender));

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.UtcNow)
                .When(x => x.BirthDate.HasValue)
                .WithMessage("Birth date must be in the past.");

            RuleFor(x => x.Address)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.Address))
                .WithMessage("Address max length is 200.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive field cannot be null.");

            RuleFor(x => x.IsBlocked)
                .NotNull().WithMessage("IsBlocked field cannot be null.");
        }
    }
}
