using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Infrastructure.Security.Settings;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public UserRefreshTokenRepository(
            ApplicationDbContext context,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<UserRefreshToken?> CreateAsync(int userId, string token, string refreshToken)
        {
            var now = DateTime.UtcNow;

            var userRefreshToken = new UserRefreshToken
            {
                UserId = userId,
                JwtId = token,
                RefreshTokenHash = refreshToken,
                CreatedAt = now,
                ExpirationDate = now.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                IsRevoked = false
            };

            await _context.UserRefreshTokens.AddAsync(userRefreshToken);

            return userRefreshToken;
        }

        public async Task<UserRefreshToken?> GetByRefreshTokenHashAsync(string refreshToken)
        {
            return await _context.UserRefreshTokens
                .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken);
        }

        public async Task<UserRefreshToken?> UpdateAsync(UserRefreshToken entity)
        {
            
            _context.UserRefreshTokens.Update(entity);
            return await Task.FromResult<UserRefreshToken?>(entity);
        }
    }
}
