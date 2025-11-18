using Authentication.RefreshToken.Application.Dto.Auth;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Auth.Command.RefreshToken
{
    public sealed record class RefreshTokenCommand : IRequest<Response<TokenInfoDto>>
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
