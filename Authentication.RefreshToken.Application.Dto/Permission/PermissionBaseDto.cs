namespace Authentication.RefreshToken.Application.Dto.Permission
{
    public class PermissionBaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
