using Authentication.RefreshToken.Application.Dto.Common;

namespace Authentication.RefreshToken.Application.Dto.Permission
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public AuditInfoDto Audit { get; set; } = null!;
    }
}
