using Authentication.RefreshToken.Application.Dto.Permission;

namespace Authentication.RefreshToken.Application.Dto.Role
{
    public record class RoleBaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
