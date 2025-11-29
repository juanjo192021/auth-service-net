using Authentication.RefreshToken.Application.Dto.Common;

namespace Authentication.RefreshToken.Application.Dto.Role
{
    public sealed record class RoleDto : RoleBase
    {
        public AuditInfoDto Audit { get; set; } = null!;
        public IEnumerable<string> Users { get; set; } = null!;
        public IEnumerable<string> Permissions { get; set; } = null!;
    }
}
