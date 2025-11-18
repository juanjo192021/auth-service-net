using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUserRefreshTokenRepository
    {
        Task<string> CreateAsync(int userId, string token, string refreshToken);
        Task<UserRefreshToken?> FindByRefreshTokenAsync(string refreshToken);
        Task UpdateAsync(UserRefreshToken userRefreshToken);
    }
}
