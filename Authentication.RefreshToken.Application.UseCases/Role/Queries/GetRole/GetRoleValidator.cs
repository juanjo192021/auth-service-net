using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRole
{
    public class GetRoleValidator : AbstractValidator<GetRoleQuery>
    {
        public GetRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("The role Id must not be empty.")
                .NotNull().WithMessage("The role Id must not be null.")
                .GreaterThan(0).WithMessage("The role Id must be greater than zero.");
        }
    }
}
