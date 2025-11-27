using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class UserPermission : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsEnabled { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}
