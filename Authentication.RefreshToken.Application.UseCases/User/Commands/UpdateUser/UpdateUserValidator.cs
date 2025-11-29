using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotNull().WithMessage("Id cannot be null.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.FirstName)
                .MaximumLength(30).WithMessage("First name max length is 30.");

            RuleFor(x => x.LastName)
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
        }
    }
}
