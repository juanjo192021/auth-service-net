using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole
{
    public sealed record class UpdateRoleCommand : IRequest<SuccessResponse<RoleDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
