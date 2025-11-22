using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh
{
    public sealed record class RefreshCommand : IRequest<SuccessResponse<TokenInfoDto>>
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
