using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class RoleClaim: BaseAuditableEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool IsEnabled { get; set; }

        // Propiedad de Navegación
        public virtual Role Role { get; set; }
    }
}
