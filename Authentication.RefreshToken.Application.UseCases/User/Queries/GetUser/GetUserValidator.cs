using FluentValidation;

namespace Authentication.RefreshToken.Application.UseCases.User.Queries.GetUser
{
    public class GetUserValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id must not be empty.")
                .NotNull().WithMessage("Id must not be null.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}
