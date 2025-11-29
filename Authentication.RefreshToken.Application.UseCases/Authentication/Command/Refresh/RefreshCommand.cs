using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh
{
    public sealed record class RefreshCommand : IRequest<ApiResponse<TokenInfoDto>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
