using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRole
{
    public class GetAllRoleValidator : AbstractValidator<GetAllRoleQuery>
    {
        public GetAllRoleValidator()
        {
            RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber debe ser mayor que 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize debe ser mayor que 0.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search no debe superar los 100 caracteres.");
        }
    }
}
