using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetAllPermission
{
    public sealed record class GetAllPermissionQuery : IRequest<ApiResponse<IEnumerable<PermissionSummaryDto>>>
    {
    }
}
