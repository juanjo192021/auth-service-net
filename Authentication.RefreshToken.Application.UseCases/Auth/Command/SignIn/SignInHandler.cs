using Authentication.RefreshToken.Application.Dto.Auth;
using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Auth.Command.SignIn
{
    public class SignInHandler : IRequestHandler<SignInCommand, Response<AuthDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public SignInHandler(IUnitOfWork unitOfWork, 
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

        public async Task<Response<AuthDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<AuthDto>();
            
            var email = request.Email;
            var password = request.Password;

            var user = await _unitOfWork.Users.GetByEmailAsync(email)
                ?? throw new NotFoundException($"Not found user with email {email}");

            user.PasswordHash = _passwordHasher.Hash(password);

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

            response.IsSuccess = true;
            response.Data = new AuthDto
            {
                Tokens = new TokenInfoDto
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                },
                User = _mapper.Map<UserDto>(user)
            };
            response.Message = "User signed in successfully.";

            return response;

        }
    }
}
