namespace Authentication.RefreshToken.Application.Dto.Authentication
{
    public sealed record class TokenInfoDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
