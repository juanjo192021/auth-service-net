namespace Authentication.RefreshToken.Application.Dto.Authentication
{
    public sealed record class AuthenticationDto
    {
        public TokenInfoDto Tokens { get; set; } = null!;
        public UserInfoDto User { get; set; } = null!;
    }
}
