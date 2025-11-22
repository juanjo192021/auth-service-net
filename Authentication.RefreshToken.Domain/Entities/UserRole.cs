using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class UserRole : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsAssigned { get; set; }

        // Propiedades de Navegación
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
