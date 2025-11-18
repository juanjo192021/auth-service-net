namespace Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security
{
    public interface IRefreshTokenService
    {
        public string GenerateRefreshToken();
        public string ComputeSha256(string raw);
    }
}
