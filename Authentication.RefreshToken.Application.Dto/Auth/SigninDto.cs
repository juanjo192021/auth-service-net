namespace Authentication.RefreshToken.Application.Dto.Auth
{
    public sealed record class SigninDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
