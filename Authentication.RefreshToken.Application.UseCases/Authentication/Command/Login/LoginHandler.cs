using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Authentication.Common;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, ApiResponse<TokenInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginHandler(IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher, 
            IJwtService jwtService, 
            IRefreshTokenService refreshTokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<ApiResponse<TokenInfoDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserWithRolesAsync(request.Email);

            ValidateCredentials(request.Password, user);

            var roles = UserRoleHelper.ExtractUserRoles(user);

            var (token, refreshToken, jwtId, refreshTokenHash) = GenerateTokens(user, roles);

            await SaveRefreshTokenAsync(user.Id, jwtId, refreshTokenHash);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<TokenInfoDto>(new TokenInfoDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            }, "User signed in successfully.");
        }

        private async Task<Domain.Entities.User> GetUserWithRolesAsync(string email)
        {
            return await _unitOfWork.Users.GetWithRolesByEmailAsync(email)
                ?? throw new NotFoundException($"User with email {email} not found.");
        }

        private void ValidateCredentials(string password, Domain.Entities.User user)
        {
            if (!user.IsActive)
                throw new UnauthorizedException("User account is not active.");

            if (user.IsBlocked)
                throw new UnauthorizedException("User account is blocked.");

            if (!_passwordHasher.Verify(password, user.PasswordHash))
                throw new UnauthorizedException("Invalid credentials.");
        }

        private GeneratedTokenData GenerateTokens(Domain.Entities.User user, List<string> roles)
        {
            var token = _jwtService.GenerateToken(user.Id, roles)
                ?? throw new Exception("Failed to generate JWT token.");

            var refreshToken = _refreshTokenService.GenerateRefreshToken()
                ?? throw new Exception("Failed to generate refresh token.");

            var jwtId = _jwtService.GetJwtId(token);
            var refreshTokenHash = _refreshTokenService.ComputeSha256(refreshToken);

            return new GeneratedTokenData(token, refreshToken, jwtId, refreshTokenHash);
        }

        private async Task SaveRefreshTokenAsync(int userId, string jwtId, string refreshTokenHash)
        {
            var saved = await _unitOfWork.UserRefreshTokens
                .CreateAsync(userId, jwtId, refreshTokenHash);

            if (saved is null)
                throw new Exception("Failed to save refresh token.");
        }
    }
}
