using Authentication.RefreshToken.Application.Dto.Auth;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Auth.Command.SignIn
{
    public sealed record class SignInCommand : IRequest<Response<AuthDto>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
