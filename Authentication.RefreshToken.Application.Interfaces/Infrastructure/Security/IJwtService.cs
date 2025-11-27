using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security
{
    public interface IJwtService
    {
        string GenerateToken(int id, List<string> roles);
        TokenStatus ValidateToken(string token);
        string GetJwtId(string token);
        int? GetUserIdFromExpiredToken(string token);
    }
}
