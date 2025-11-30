using Authentication.RefreshToken.Application.Dto.Common;

namespace Authentication.RefreshToken.Application.Dto.Permission
{
    public class PermissionDto
    {
        public AuditDto Audit { get; set; } = null!;
    }
}
