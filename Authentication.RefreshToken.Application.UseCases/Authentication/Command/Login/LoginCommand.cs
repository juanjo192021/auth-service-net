using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login
{
    public sealed record class LoginCommand : IRequest<SuccessResponse<AuthenticationDto>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
