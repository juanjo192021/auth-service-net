using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class Permission : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
        public virtual ICollection<UserPermission>? UserPermissions { get; set; }
        public virtual ICollection<Menu>? Menus { get; set; }
    }
}
