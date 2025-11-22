namespace Authentication.RefreshToken.Application.Dto.Authentication
{
    public sealed record class UserInfoDto
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
