using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class Customer: BaseAuditableEntity
    {
        public int Id { get; set; }
        public string? CustomerType { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; } 
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? BillingAddress { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingPreferences { get; set; }
        public decimal? CreditLimit { get; set; }
        public string? CustomerSegment { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
