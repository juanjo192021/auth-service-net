using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        //public bool EmailMfaEnabled { get; set; }
        //public bool SmsMfaEnabled { get; set; }
        //public bool TotpMfaEnabled { get; set; }
        //public string? PreferredMfaMethod { get; set; }
        //public string? TotpSecret { get; set; }
        //public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
        public virtual ICollection<UserClaim>? UserClaims { get; set; } 
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public virtual ICollection<UserPermission>? UserPermissions { get; set; }
    }
}
