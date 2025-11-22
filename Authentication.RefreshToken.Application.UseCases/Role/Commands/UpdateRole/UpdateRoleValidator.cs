using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Role Id is required.")
                .NotNull().WithMessage("Role Id cannot be null.")
                .GreaterThan(0).WithMessage("Role Id must be greater than zero.");
        }
    }
}
