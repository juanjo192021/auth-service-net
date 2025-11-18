using Authentication.RefreshToken.Application.Dto.Auth;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Auth.Command.SignUp
{
    public sealed record class SignUpCommand : IRequest<Response<AuthDto>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
