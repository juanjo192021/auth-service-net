using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class Menu : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!; 
        public string Route { get; set; } = null!;
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int? PermissionId { get; set; }
        public virtual Permission? Permission { get; set; }
        public virtual ICollection<Menu>? Children { get; set; }
    }
}
