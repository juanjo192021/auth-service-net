namespace Authentication.RefreshToken.Domain.Entities
{
    public class UserTwoFactorCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = null!;
        public string Method { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }
        public virtual User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
