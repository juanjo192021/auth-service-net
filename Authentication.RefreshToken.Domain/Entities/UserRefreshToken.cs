using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class UserRefreshToken: BaseEntity
    {
        public int UserId { get; set; }
        public string JwtId { get; set; }
        public string RefreshTokenHash { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Propiedad de Navegación
        public virtual User User { get; set; }
    }
}
