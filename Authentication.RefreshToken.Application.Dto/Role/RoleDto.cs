namespace Authentication.RefreshToken.Application.Dto.Role
{
    public sealed record class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public int? DeactivatedBy { get; set; }
        public IEnumerable<string> Users { get; set; } = null!;
    }
}
