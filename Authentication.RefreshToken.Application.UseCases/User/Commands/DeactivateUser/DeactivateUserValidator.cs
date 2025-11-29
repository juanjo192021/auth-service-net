using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.DeactivateUser
{
    public class DeactivateUserValidator : AbstractValidator<DeactivateUserCommand>
    {
        public DeactivateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotNull().WithMessage("Id cannot be null.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}
