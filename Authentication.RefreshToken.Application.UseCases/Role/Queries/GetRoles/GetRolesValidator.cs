using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRoles
{
    public class GetRolesValidator : AbstractValidator<GetRolesQuery>
    {
        public GetRolesValidator()
        {
            RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");

            RuleFor(x => x.Search)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search must not exceed 50 characters.");
        }
    }
}
