using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUserRefreshTokenRepository
    {
        Task<UserRefreshToken?> CreateAsync(int userId, string token, string refreshToken);
        Task<UserRefreshToken?> GetByRefreshTokenHashAsync(string refreshToken);
        Task<UserRefreshToken?> UpdateAsync(UserRefreshToken userRefreshToken);
    }
}
