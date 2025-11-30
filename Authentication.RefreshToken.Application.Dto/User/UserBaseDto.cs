namespace Authentication.RefreshToken.Application.Dto.User
{
    public record class UserBaseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
    }
}
