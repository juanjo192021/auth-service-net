namespace Authentication.RefreshToken.Domain.Entities
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public int? DeactivatedBy { get; set; }

        // Propiedades de Navegación
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
