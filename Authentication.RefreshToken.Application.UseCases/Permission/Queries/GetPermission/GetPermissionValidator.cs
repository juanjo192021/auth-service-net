using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermission
{
    public class GetPermissionValidator : AbstractValidator<GetPermissionQuery>
    {
        public GetPermissionValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("The role Id must not be empty.")
                .NotNull().WithMessage("The role Id must not be null.")
                .GreaterThan(0).WithMessage("The role Id must be greater than zero.");
        }
    }
}
