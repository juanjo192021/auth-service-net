using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermissions
{
    public sealed record class GetPermissionsQuery : IRequest<PagedResponse<IEnumerable<PermissionDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; } = string.Empty;
    }
}
