using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Domain.Constants;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, ApiResponse<TokenInfoDto>>
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

        public async Task<ApiResponse<TokenInfoDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<TokenInfoDto>();

            await EnsureEmailNotTaken(request.Email);

            var basicRole = await GetDefaultRole();

            var userCreated = await CreateUser(request, cancellationToken);

            await AssignBasicRole(userCreated.Id, basicRole.Id, cancellationToken);

            var tokens = await GenerateAndStoreTokens(userCreated.Id, basicRole.Name, cancellationToken);

            return new ApiResponse<TokenInfoDto>(tokens, "User registered successfully.");
        }

        private async Task EnsureEmailNotTaken(string email)
        {
            if (!await _unitOfWork.Users.IsEmailUniqueAsync(email))
                throw new ConflictException($"User with email {email} already exists.");
        }

        private async Task<Domain.Entities.Role> GetDefaultRole()
        {
            return await _unitOfWork.Roles.GetByNameAsync(Roles.BasicUser)
                ?? throw new NotFoundException("Default BASIC_USER role was not found.");
        }

        private async Task<Domain.Entities.User> CreateUser(RegisterCommand request, CancellationToken ct)
        {
            var user = _mapper.Map<Domain.Entities.User>(request);

            user.IsActive = true;
            user.IsBlocked = false;
            user.PasswordHash = _passwordHasher.Hash(request.Password);

            var created = await _unitOfWork.Users.CreateAsync(user)
                ?? throw new Exception("Failed to create user.");

            await _unitOfWork.CommitAsync(ct);

            return created;
        }

        private async Task AssignBasicRole(int userId, int roleId, CancellationToken ct)
        {
            var assigned = await _unitOfWork.UserRoles.AssignRolesAsync(userId, new List<int> { roleId });

            if (!assigned.Any())
                throw new Exception("Failed to assign role to user.");

            await _unitOfWork.CommitAsync(ct);
        }

        private async Task<TokenInfoDto> GenerateAndStoreTokens(int userId, string roleName, CancellationToken ct)
        {
            var roles = new List<string> { roleName };

            var token = _jwtService.GenerateToken(userId, roles);
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            var jwtId = _jwtService.GetJwtId(token);
            var refreshTokenHash = _refreshTokenService.ComputeSha256(refreshToken);

            var saved = await _unitOfWork.UserRefreshTokens.CreateAsync(userId, jwtId, refreshTokenHash);

            if (saved is null)
                throw new Exception("Failed to save refresh token.");

            await _unitOfWork.CommitAsync(ct);

            return new TokenInfoDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }
    }
}
