using Authentication.RefreshToken.Application.Dto.Common;

namespace Authentication.RefreshToken.Application.Dto.Permission
{
    public class PermissionDto : PermissionSummaryDto
    {
        public AuditDto Audit { get; set; } = null!;
    }
}
