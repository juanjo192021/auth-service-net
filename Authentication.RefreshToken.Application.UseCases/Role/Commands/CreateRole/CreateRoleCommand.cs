using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole
{
    public sealed record class CreateRoleCommand : IRequest<SuccessResponse<RoleDto>>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
