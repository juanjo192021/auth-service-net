using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Domain.Entities;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Authentication.RefreshToken.UnitTest.Auth
{
    public class SignInHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IUserRefreshTokenRepository> _refreshRepo = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IJwtService> _jwtService = new();
        private readonly Mock<IRefreshTokenService> _refreshTokenService = new();
        private readonly Mock<IMapper> _mapper = new();

        private LoginHandler CreateHandler()
        {
            _unitOfWork.Setup(u => u.Users).Returns(_userRepository.Object);
            _unitOfWork.Setup(u => u.UserRefreshTokens).Returns(_refreshRepo.Object);

            return new LoginHandler(
                _unitOfWork.Object,
                _passwordHasher.Object,
                _jwtService.Object,
                _refreshTokenService.Object,
                _mapper.Object
            );
        }

        // Case: User email not found
        [Fact]
        public async Task Handler_WhenUserEmailNotFound_ShouldThrowValidationException()
        {
            // Arrange
            var command = new LoginCommand { Email = "invalid@gmail.com", Password = "1234" };

            _userRepository
                .Setup(r => r.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Not found user with email {command.Email}");
        }

        // Case: Invalid password
        [Fact]
        public async Task Handler_WhenPasswordIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                PasswordHash = "hashed"
            };

            var command = new LoginCommand { Email = user.Email, Password = "wrong-password" };

            _userRepository
                .Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _passwordHasher
                .Setup(h => h.Hash(command.Password))
                .Returns("hashed");

            _passwordHasher
                .Setup(h => h.Verify(command.Password, "hashed"))
                .Returns(false);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid email or password.");
        }
        
        // Case: Token generation failure
        [Fact]
        public async Task Handler_WhenTokenCantBeGenerated_ShouldThrowException()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                PasswordHash = "hashed"
            };

            var command = new LoginCommand { Email = user.Email, Password = "wrong-password" };

            _userRepository
                .Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _passwordHasher
                .Setup(h => h.Hash(command.Password))
                .Returns("hashed");

            _passwordHasher
                .Setup(h => h.Verify(command.Password, "hashed"))
                .Returns(true);

            _jwtService.Setup(j => j.GenerateToken(user)).Returns((string)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Failed to generate JWT token.");
        }
        
        // Case: Refresh token generation failure
        [Fact]
        public async Task Handler_WhenRefreshTokenCantBeGenerated_ShouldThrowException()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                PasswordHash = "hashed"
            };

            var command = new LoginCommand { Email = user.Email, Password = "wrong-password" };

            _userRepository
                .Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _passwordHasher
                .Setup(h => h.Hash(command.Password))
                .Returns("hashed");

            _passwordHasher
                .Setup(h => h.Verify(command.Password, "hashed"))
                .Returns(true);

            _jwtService.Setup(j => j.GenerateToken(user)).Returns("jwt-token");
            _jwtService.Setup(j => j.GetJwtId("jwt-token")).Returns("jwt-id");

            _refreshTokenService.Setup(r => r.GenerateRefreshToken()).Returns((string)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Failed to generate refresh token.");
        }

        // Case: Successful authentication
        [Fact]
        public async Task Handler_WhenCredentialsAreValid_ShouldLoginSuccessfuly()
        {
            // Arrange
            var user = new User
            {
                Email = "test@gmail.com",
                PasswordHash = "hashed"
            };

            var command = new LoginCommand { Email = user.Email, Password = "1234" };

            _userRepository.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            _passwordHasher.Setup(h => h.Hash("1234")).Returns("hashed");
            _passwordHasher.Setup(h => h.Verify("1234", "hashed")).Returns(true);

            _jwtService.Setup(j => j.GenerateToken(user)).Returns("jwt-token");
            _jwtService.Setup(j => j.GetJwtId("jwt-token")).Returns("jwt-id");

            _refreshTokenService.Setup(r => r.GenerateRefreshToken()).Returns("refresh-token");
            _refreshTokenService.Setup(r => r.ComputeSha256("refresh-token")).Returns("refresh-token-hash");

            _mapper.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Email = user.Email });

            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Tokens.AccessToken.Should().Be("jwt-token");
            result.Data.Tokens.RefreshToken.Should().Be("refresh-token");
            result.Data.User.Email.Should().Be(user.Email);
            result.Message.Should().Be("User signed in successfully.");

            _refreshRepo.Verify(r =>
                r.CreateAsync(user.Id, "jwt-id", "refresh-token-hash"),
                Times.Once);
        }
    }
}
