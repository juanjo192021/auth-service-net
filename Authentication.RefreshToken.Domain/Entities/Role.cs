using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class Role: BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // Propiedades de Navegación
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
