using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.RevokeToken
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, ApiResponse<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenService _refreshTokenService;

        public RevokeTokenHandler(IUnitOfWork unitOfWork, IRefreshTokenService refreshTokenService)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<ApiResponse<object>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var userRefreshToken = await ValidateRefreshTokenAsync(request);
            await RevokeRefreshTokenAsync(userRefreshToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return new ApiResponse<object>(null,"Refresh Token revoked successfully!");
        }

        private async Task <UserRefreshToken> ValidateRefreshTokenAsync(RevokeTokenCommand request)
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

            return userRefreshToken;
        }

        private async Task RevokeRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            userRefreshToken.IsRevoked = true;
            await _unitOfWork.UserRefreshTokens.UpdateAsync(userRefreshToken);
        }
    }
}
