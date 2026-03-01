using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermission
{
    public sealed record class GetPermissionQuery : IRequest<ApiResponse<PermissionDto>>
    {
        public int Id { get; set; }
    }
}
