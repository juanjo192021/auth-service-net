namespace Authentication.RefreshToken.Domain.Common
{
    public class BaseAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public int? DeactivatedBy { get; set; }
    }
}
