using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role Name is required.")
                .NotNull().WithMessage("Role Name cannot be null.")
                .MaximumLength(20).WithMessage("Role Name cannot exceed 20 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Role Description cannot exceed 100 characters.");
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive field cannot be null.");
        }
    }
}
