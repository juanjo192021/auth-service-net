using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, SuccessResponse<AuthenticationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public LoginHandler(IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher, 
            IJwtService jwtService, 
            IRefreshTokenService refreshTokenService, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<AuthenticationDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<AuthenticationDto>();
            
            var email = request.Email;
            var password = request.Password;

            var user = await _unitOfWork.Users.GetByEmailAsync(email)
                ?? throw new NotFoundException($"Not found user with email {email}");

            bool validPassword = _passwordHasher.Verify(password, user.PasswordHash);
            if (!validPassword)
                throw new UnauthorizedException("Invalid email or password.");

            var token = _jwtService.GenerateToken(user) 
                ?? throw new Exception("Failed to generate JWT token.");

            var refreshToken = _refreshTokenService.GenerateRefreshToken()
                ?? throw new Exception("Failed to generate refresh token.");

            var jwtId = _jwtService.GetJwtId(token);
            var refreshTokenHash = _refreshTokenService.ComputeSha256(refreshToken);

            await _unitOfWork.UserRefreshTokens.CreateAsync(user.Id, jwtId, refreshTokenHash);

            response.Data = new AuthenticationDto
            {
                Tokens = new TokenInfoDto
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                },
                User = _mapper.Map<UserInfoDto>(user)
            };
            response.Message = "User signed in successfully.";

            return response;

        }
    }
}
