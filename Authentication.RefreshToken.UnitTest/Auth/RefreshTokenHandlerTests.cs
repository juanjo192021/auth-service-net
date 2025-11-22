using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Authentication.RefreshToken.UnitTest.Auth
{
    public class RefreshTokenHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IUserRefreshTokenRepository> _refreshRepo = new();
        private readonly Mock<IJwtService> _jwtService = new();
        private readonly Mock<IRefreshTokenService> _refreshTokenService = new();

        private RefreshHandler CreateHandler()
        {
            _unitOfWork.Setup(u => u.Users).Returns(_userRepository.Object);
            _unitOfWork.Setup(u => u.UserRefreshTokens).Returns(_refreshRepo.Object);

            return new RefreshHandler(
                _unitOfWork.Object,
                _jwtService.Object,
                _refreshTokenService.Object
            );
        }

        // Case: Refresh token does not exist
        [Fact]
        public async Task Handler_WhenRefreshTokenNotExist_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-access-token",
                RefreshToken = "invalid-refresh-token"
            };

            var jwtId = "valid-jwt-id";
            var refreshTokenHash = "invalid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync((UserRefreshToken?)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Not found refresh token.");

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);
        }

        // Case: Refresh token is revoked
        [Fact]
        public async Task Handler_WhenRefreshTokenIsRevoked_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-access-token",
                RefreshToken = "valid-refresh-token"
            };

            var jwtId = "valid-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = 12,
                JwtId = jwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                IsRevoked = true
            };

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Refresh token was revoked.");

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);
        }

        // Case: JWT ID does not match stored JWT ID
        [Fact]
        public async Task Handler_WhenJwtIdDoesNotMatchStoredJwtId_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "invalid-access-token",
                RefreshToken = "valid-refresh-token"
            };

            var jwtId = "invalid-jwt-id";
            var StorejwtId = "valid-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = 12,
                JwtId = StorejwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                IsRevoked = false
            };

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Access token does not match the refresh token.");

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);
        }

        // Case: Refresh token is expired
        [Fact]
        public async Task Handler_WhenRefreshTokenIsExpired_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-access-token",
                RefreshToken = "valid-refresh-token"
            };

            var jwtId = "valid-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            var timeExpired = DateTime.UtcNow.AddMinutes(-5);
            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = 12,
                JwtId = jwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = timeExpired,
                IsRevoked = false
            };

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Refresh token has expired.");

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);
        }

        // Case: Token is valid but not expired
        [Fact]
        public async Task Handler_WhenTokenHasNotExpired_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-access-token",
                RefreshToken = "valid-refresh-token"
            };

            var jwtId = "valid-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = 12,
                JwtId = jwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                IsRevoked = false
            };

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("The token is still valid, no need to renew it.");


            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);
            _jwtService.Verify(j => j.ValidateToken(command.AccessToken), Times.Once);
        }

        [Fact]
        public async Task Handler_WhenUserIdOfJwtIdIsNotValid_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-format-token",
                RefreshToken = "valid-refresh-token"
            };

            var jwtId = "valid-format-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);


            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = 12,
                JwtId = jwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                IsRevoked = false
            };

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            _jwtService
                .Setup(j => j.ValidateToken(command.AccessToken))
                .Returns(TokenStatus.Expired);

            _jwtService
                .Setup(j => j.GetUserIdFromExpiredToken(command.AccessToken))
                .Returns((int?)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid or corrupt token, could not retrieve user ID.");

            _refreshRepo.Verify(urf => urf.FindByRefreshTokenAsync(It.IsAny<string>()), Times.Once);

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);

            _jwtService.Verify(j => j.ValidateToken(command.AccessToken), Times.Once);
            _jwtService.Verify(j => j.GetUserIdFromExpiredToken(command.AccessToken), Times.Once);

        }

        // Case: User ID from JWT not found
        [Fact]
        public async Task Handler_WhenUserIdOfJwtIdNotFound_ShouldThrowException()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-format-token",
                RefreshToken = "valid-refresh-token"
            };

            var jwtId = "valid-format-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            
            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = 12,
                JwtId = jwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                IsRevoked = false
            };
            var userIdClaims = storedToken.Id;

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            _jwtService
                .Setup(j => j.ValidateToken(command.AccessToken))
                .Returns(TokenStatus.Expired);

            _jwtService
                .Setup(j => j.GetUserIdFromExpiredToken(command.AccessToken))
                .Returns(userIdClaims);

            _userRepository
                .Setup(r => r.GetByIdAsync(userIdClaims))
                .ReturnsAsync((User?)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Not found user");

            _refreshRepo.Verify(urf => urf.FindByRefreshTokenAsync(It.IsAny<string>()), Times.Once);

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);

            _jwtService.Verify(j => j.ValidateToken(command.AccessToken), Times.Once);
            _jwtService.Verify(j => j.GetUserIdFromExpiredToken(command.AccessToken), Times.Once);

            _userRepository.Verify(r => r.GetByIdAsync(userIdClaims), Times.Once);
        }

        // Case: Valid refresh token and access token
        [Fact]
        public async Task Handler_WhenRefreshTokenIsValid_ShouldReturnNewTokens()
        {
            // Arrange
            var command = new RefreshCommand
            {
                AccessToken = "valid-access-token",
                RefreshToken = "valid-refresh-token"
            };
            var jwtId = "valid-jwt-id";
            var refreshTokenHash = "valid-refresh-token-hash";

            _jwtService
                .Setup(j => j.GetJwtId(command.AccessToken))
                .Returns(jwtId);

            _refreshTokenService
                .Setup(r => r.ComputeSha256(command.RefreshToken))
                .Returns(refreshTokenHash);

            var user = new User { Id = 1, Email = "test@gmail.com" };

            var storedToken = new UserRefreshToken
            {
                Id = 10,
                UserId = user.Id,
                JwtId = jwtId,
                RefreshTokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                IsRevoked = false
            };

            _refreshRepo
                .Setup(r => r.FindByRefreshTokenAsync(refreshTokenHash))
                .ReturnsAsync(storedToken);

            _jwtService
                .Setup(j => j.ValidateToken(command.AccessToken))
                .Returns(TokenStatus.Expired);

            _jwtService
                .Setup(j => j.GetUserIdFromExpiredToken(command.AccessToken))
                .Returns(user.Id);

            _userRepository
                .Setup(r => r.GetByIdAsync(user.Id))
                .ReturnsAsync(user);

            var newAccessToken = "new-access-token";
            var newJwtId = "new-jwt-id";

            _jwtService
                .Setup(j => j.GenerateToken(user))
                .Returns(newAccessToken);

            _jwtService.Setup(j => j.GetJwtId(newAccessToken)).Returns(newJwtId);

            var newRefreshToken = "new-refresh-token";
            var RefreshTokenHash = "new-refresh-token-hash";

            _refreshTokenService
                .Setup(r => r.GenerateRefreshToken())
                .Returns(newRefreshToken);

            _refreshTokenService.Setup(rt => rt.ComputeSha256(newRefreshToken)).Returns(RefreshTokenHash);

            _refreshRepo.Setup(urt => urt.CreateAsync(user.Id, newJwtId, RefreshTokenHash))
                        .Returns(Task.FromResult<string>(RefreshTokenHash));

            var handler = CreateHandler();

            // Act
            var response = await handler.Handle(command, default);

            // Assert
            response.Should().NotBeNull("Ta Bien");
            response.IsSuccess.Should().BeTrue();
            response.Data.AccessToken.Should().Be(newAccessToken);
            response.Data.RefreshToken.Should().Be(newRefreshToken);
            response.Message.Should().Be("Token refreshed successfully.");
            storedToken.IsRevoked.Should().BeTrue();

            _refreshRepo.Verify(r => r.FindByRefreshTokenAsync(refreshTokenHash), Times.Once);
            _refreshRepo.Verify(r => r.UpdateAsync(storedToken), Times.Once);
            _refreshRepo.Verify(r => r.CreateAsync(user.Id, newJwtId, RefreshTokenHash), Times.Once);

            _jwtService.Verify(j => j.ValidateToken(command.AccessToken), Times.Once);
            _jwtService.Verify(j => j.GetUserIdFromExpiredToken(command.AccessToken), Times.Once);
            _jwtService.Verify(j => j.GenerateToken(user), Times.Once);

            _userRepository.Verify(r => r.GetByIdAsync(user.Id), Times.Once);

            _refreshTokenService.Verify(r => r.GenerateRefreshToken(), Times.Once);
        }

    }
}
