using Authentication.RefreshToken.Application.Dto.Account;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Account.Queries.GetCurrentUser
{
    public sealed record class GetCurrentUserQuery : IRequest<ApiResponse<AccountDto>>
    {
    }
}
