using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Domain.Enums;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, SuccessResponse<AuthenticationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public RegisterHandler(
            IUnitOfWork unitOfWork, 
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

        public async Task<SuccessResponse<AuthenticationDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<AuthenticationDto>();

            var userExists = await _unitOfWork.Users.GetByEmailAsync(request.Email);
                
            if (userExists != null)
                throw new ConflictException($"User with email {request.Email} already exists.");

            var newUser = _mapper.Map<User>(request);
            newUser.IsActive = true;
            newUser.IsBlocked = false;
            newUser.PasswordHash = _passwordHasher.Hash(request.Password);

            var userCreated = await _unitOfWork.Users.CreateAsync(newUser)
                ?? throw new Exception("Failed to create user.");

            var basicRole = await _unitOfWork.Roles.GetByNameAsync(DefaultRoles.BasicUser.Name)
                ?? throw new NotFoundException("Default BASIC_USER role was not found.");

            var added = await _unitOfWork.UserRoles.CreateAsync(userCreated.Id, new List<int> { basicRole.Id });

            if (added == 0)
                throw new Exception("Failed to assign role to user.");

            var user = await _unitOfWork.Users.GetByIdAsync(userCreated.Id)
                ?? throw new NotFoundException("Failed to retrieve user after creation.");

            var token = _jwtService.GenerateToken(user)
                ?? throw new Exception("Failed to generate JWT token.");

            var refreshToken = _refreshTokenService.GenerateRefreshToken()
                ?? throw new Exception("Failed to generate refresh token.");

            var jwtId = _jwtService.GetJwtId(token);

            var refreshTokenHash = _refreshTokenService.ComputeSha256(refreshToken);

            await _unitOfWork.UserRefreshTokens.CreateAsync(userCreated.Id, jwtId, refreshTokenHash);

            //await _unitOfWork.CommitAsync();

            response.Data = new AuthenticationDto
            {
                Tokens = new TokenInfoDto
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                },
                User = _mapper.Map<UserInfoDto>(user)
            };
            response.Message = "User registered successfully.";

            return response;
        }
    }
}
