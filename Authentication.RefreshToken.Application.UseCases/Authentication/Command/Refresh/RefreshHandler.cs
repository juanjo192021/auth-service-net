using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, SuccessResponse<TokenInfoDto>>
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

        public async Task<SuccessResponse<TokenInfoDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<TokenInfoDto>();
            
            var jwtId = _jwtService.GetJwtId(request.AccessToken);
            var resetTokenHash = _refreshTokenService.ComputeSha256(request.RefreshToken);

            var tokenEntity = await _unitOfWork.UserRefreshTokens.FindByRefreshTokenAsync(resetTokenHash) 
                ?? throw new NotFoundException("Not found refresh token.");

            if (tokenEntity.IsRevoked)
                throw new UnauthorizedException("Refresh token was revoked.");
            
            if (tokenEntity.JwtId != jwtId)
                throw new UnauthorizedException("Access token does not match the refresh token.");
            

            if (tokenEntity.ExpirationDate <= DateTime.UtcNow)
            {
                tokenEntity.IsRevoked = true;
                await _unitOfWork.UserRefreshTokens.UpdateAsync(tokenEntity);

                throw new UnauthorizedException("Refresh token has expired.");
            }

            var tokenStatus = _jwtService.ValidateToken(request.AccessToken);

            switch (tokenStatus)
            {
                case TokenStatus.Valid:
                    throw new UnauthorizedException("The token is still valid, no need to renew it.");
                case TokenStatus.InvalidFormat:
                    throw new UnauthorizedException("The token format is invalid.");
                case TokenStatus.InvalidSignature:
                    throw new UnauthorizedException("The token was tampered with or the signature is invalid.");
                case TokenStatus.Corrupt:
                    throw new UnauthorizedException("The token is corrupt.");
                case TokenStatus.Expired:
                    break;
            }

            // Obtener los claims del token para la renovación
            var userIdClaim = _jwtService.GetUserIdFromExpiredToken(request.AccessToken)
                ?? throw new Exception("Invalid or corrupt token, could not retrieve user ID.");

            var user = await _unitOfWork.Users.GetByIdAsync(userIdClaim)
                ?? throw new NotFoundException("Not found user");
            
            // Invalidar el refresh token anterior
            tokenEntity.IsRevoked = true;
            await _unitOfWork.UserRefreshTokens.UpdateAsync(tokenEntity);

            // Generar nuevos tokens
            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            var jwtIdNew = _jwtService.GetJwtId(newToken);
            var newRefreshTokenHash = _refreshTokenService.ComputeSha256(newRefreshToken);

            await _unitOfWork.UserRefreshTokens.CreateAsync(user.Id, jwtIdNew, newRefreshTokenHash);

            response.Data = new TokenInfoDto
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken
            };
            response.Message = "Token refreshed successfully.";

            return response;
        }
    }
}
