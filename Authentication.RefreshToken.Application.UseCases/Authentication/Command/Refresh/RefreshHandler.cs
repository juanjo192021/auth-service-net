using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Authentication.Common;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, ApiResponse<TokenInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshHandler(
            IUnitOfWork unitOfWork, 
            IJwtService jwtService, 
            IRefreshTokenService refreshTokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<ApiResponse<TokenInfoDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {

            var (refreshEntity, user) = await ValidateRefreshTokenAsync(request);

            var roles = UserRoleHelper.ExtractUserRoles(user);

            await RevokeRefreshTokenAsync(refreshEntity);

            var tokens = GenerateTokens(user, roles);

            await SaveRefreshTokenAsync(user.Id, tokens.JwtId, tokens.RefreshTokenHash);

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<TokenInfoDto>(new TokenInfoDto
            {
                AccessToken = tokens.Token,
                RefreshToken = tokens.RefreshToken
            },"Token refreshed successfully.");
        }

        private async Task<(UserRefreshToken RefreshEntity, Domain.Entities.User User)> ValidateRefreshTokenAsync(RefreshCommand request)
        {
            var refreshHash = _refreshTokenService.ComputeSha256(request.RefreshToken);

            var userRefreshToken = await _unitOfWork.UserRefreshTokens.GetByRefreshTokenHashAsync(refreshHash)
                ?? throw new NotFoundException("Refresh token not found.");

            if (userRefreshToken.IsRevoked)
                throw new UnauthorizedException("Refresh token was revoked.");

            if (userRefreshToken.ExpirationDate <= DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                await _unitOfWork.UserRefreshTokens.UpdateAsync(userRefreshToken);
                throw new UnauthorizedException("Refresh token expired.");
            }

            var user = await _unitOfWork.Users.GetWithRolesByIdAsync(userRefreshToken.UserId)
                ?? throw new NotFoundException("User not found.");

            return (userRefreshToken, user);
        }

        private async Task RevokeRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            userRefreshToken.IsRevoked = true;
            await _unitOfWork.UserRefreshTokens.UpdateAsync(userRefreshToken);
        }

        private GeneratedTokenData GenerateTokens(Domain.Entities.User user, List<string> roles)
        {
            var token = _jwtService.GenerateToken(user.Id, roles);

            var refresh = _refreshTokenService.GenerateRefreshToken();

            var jwtId = _jwtService.GetJwtId(token);

            var refreshHash = _refreshTokenService.ComputeSha256(refresh);

            return new GeneratedTokenData(token, refresh, jwtId, refreshHash);
        }

        private async Task SaveRefreshTokenAsync(int userId, string jwtId, string tokenHash)
        {
            var saved = await _unitOfWork.UserRefreshTokens.CreateAsync(userId, jwtId, tokenHash);
            if (saved == null)
                throw new Exception("Failed to store new refresh token.");
        }
    }
}
