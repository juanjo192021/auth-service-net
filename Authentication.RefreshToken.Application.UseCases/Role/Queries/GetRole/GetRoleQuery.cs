using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRole
{
    public sealed record class GetRoleQuery : IRequest<SuccessResponse<RoleDto>>
    {
        public int Id { get; set; }
    }
}
