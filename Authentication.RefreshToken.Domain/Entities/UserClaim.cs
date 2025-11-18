using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class UserClaim: BaseAuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool IsEnabled { get; set; }

        // Propiedad de Navegación
        public virtual User User { get; set; }
    }
}
