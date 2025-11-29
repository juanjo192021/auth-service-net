using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.RevokeToken
{
    public sealed record class RevokeTokenCommand : IRequest<ApiResponse<object>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
