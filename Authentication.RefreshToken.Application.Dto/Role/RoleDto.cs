using Authentication.RefreshToken.Application.Dto.Common;
using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Dto.User;

namespace Authentication.RefreshToken.Application.Dto.Role
{
    public sealed record class RoleDto : RoleBaseDto
    {
        public AuditDto Audit { get; set; } = null!;
        public IEnumerable<UserSummaryDto> Users { get; set; } = null!;
        public IEnumerable<PermissionSummaryDto> Permissions { get; set; } = null!;
    }
}
