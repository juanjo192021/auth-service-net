using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermissionLookup
{
    public class GetPermissionLookupQuery : IRequest<ApiResponse<IEnumerable<PermissionSummaryDto>>>
    {
    }
}
