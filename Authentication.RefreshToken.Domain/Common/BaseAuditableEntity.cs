using Authentication.RefreshToken.Domain.Entities;

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
        public virtual User? CreatedByUser { get; set; }
        public virtual User? UpdateByUser { get; set; }
        public virtual User? DeactivatedByUser { get; set; }
    }
}
