using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRoleWithoutPagination
{
    public sealed record class GetAllRoleWithoutPaginationQuery : IRequest<ApiResponse<IEnumerable<RoleSummaryDto>>>
    {
    }
}
