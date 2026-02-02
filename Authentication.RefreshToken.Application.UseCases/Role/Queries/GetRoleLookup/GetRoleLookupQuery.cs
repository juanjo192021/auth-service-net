using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRoleLookup
{
    public sealed record class GetRoleLookupQuery : IRequest<ApiResponse<IEnumerable<RoleSummaryDto>>>
    {
    }
}
