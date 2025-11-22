namespace Authentication.RefreshToken.Application.Dto.Authentication
{
    public sealed record class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
