using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Domain.Enums;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Authentication.RefreshToken.UnitTest.Auth
{
    public class SignUpHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IRoleRepository> _rolesRepository = new();
        private readonly Mock<IUserRoleRepository> _userRoleRepo = new();
        private readonly Mock<IUserRefreshTokenRepository> _refreshRepo = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IJwtService> _jwtService = new();
        private readonly Mock<IRefreshTokenService> _refreshTokenService = new();
        private readonly Mock<IMapper> _mapper = new();

        private RegisterHandler CreateHandler()
        {
            _unitOfWork.Setup(u => u.Users).Returns(_userRepository.Object);
            _unitOfWork.Setup(u => u.Roles).Returns(_rolesRepository.Object);
            _unitOfWork.Setup(u => u.UserRoles).Returns(_userRoleRepo.Object);
            _unitOfWork.Setup(u => u.UserRefreshTokens).Returns(_refreshRepo.Object);

            return new RegisterHandler(
                _unitOfWork.Object,
                _passwordHasher.Object,
                _jwtService.Object,
                _refreshTokenService.Object,
                _mapper.Object
            );
        }

        // Case: User email exists 
        [Fact]
        public async Task Handle_WhenUserEmailAlreadyExists_ShouldThrowValidationException()
        {
            // Arrange
            var command = new RegisterCommand { Email = "test@gmail.com", Password = "123456" };

            var userExisting = new User { Id = 1, Email = "test@gmail.com" };

            _userRepository
                .Setup(r => r.GetByEmailAsync(command.Email))
                .ReturnsAsync(userExisting);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage($"User with email {command.Email} already exists.");

            _userRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        // Case: Basic Role not found 
        [Fact]
        public async Task Handler_WhenBasicRolNotFound_ShouldThrowValidationException()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "John",
                LastName = "Test"
            };

            _userRepository
                .Setup(u => u.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var mappedUser = new User { Email = command.Email };
            _mapper.Setup(m => m.Map<User>(command)).Returns(mappedUser);

            mappedUser.IsActive = true;
            mappedUser.IsBlocked = false;

            _passwordHasher.Setup(ph => ph.Hash(command.Password)).Returns("hashed");

            var newUser = new User { Id = 1, Email = command.Email };
            _userRepository.Setup(u => u.CreateAsync(mappedUser)).ReturnsAsync(newUser);

            _rolesRepository.Setup(r => r.FindByNameAsync(DefaultRoles.BasicUser.Name))
                       .ReturnsAsync((Role?)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Default BASIC_USER role was not found.");

            _userRepository.Verify(u => u.CreateAsync(mappedUser), Times.Once);
        }

        // Case: Role can't be assigned for user
        [Fact]
        public async Task Handler_WhenUserRoleCannotBeAssigned_ShouldThrowException()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "John",
                LastName = "Test"
            };

            _userRepository
                .Setup(u => u.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var mappedUser = new User { Email = command.Email };
            _mapper.Setup(m => m.Map<User>(command)).Returns(mappedUser);

            mappedUser.IsActive = true;
            mappedUser.IsBlocked = false;

            _passwordHasher.Setup(ph => ph.Hash(command.Password)).Returns("hashed");

            var newUser = new User { Id = 1, Email = command.Email };
            _userRepository.Setup(u => u.CreateAsync(mappedUser)).ReturnsAsync(newUser);

            var basicRole = new Role { Id = 4, Name = "BASIC_USER" };
            _rolesRepository.Setup(r => r.FindByNameAsync(DefaultRoles.BasicUser.Name))
                       .ReturnsAsync(basicRole);

            _userRoleRepo.Setup(ur => ur.CreateAsync(newUser.Id, It.IsAny<List<int>>()))
              .ReturnsAsync(0);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Failed to assign role to user.");

            _userRepository.Verify(u => u.CreateAsync(mappedUser), Times.Once);
            _userRoleRepo.Verify(ur => 
                ur.CreateAsync(newUser.Id, It.Is<List<int>>(l => l.Contains(basicRole.Id))), 
                Times.Once);
        }

        // Case: User not fount after created
        [Fact]
        public async Task Handler_WhenUserNotFoundAfterCreated_ShouldThrowException()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "John",
                LastName = "Test"
            };

            _userRepository
                .Setup(u => u.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var mappedUser = new User { Email = command.Email };
            _mapper.Setup(m => m.Map<User>(command)).Returns(mappedUser);

            mappedUser.IsActive = true;
            mappedUser.IsBlocked = false;

            _passwordHasher.Setup(ph => ph.Hash(command.Password)).Returns("hashed");

            var newUser = new User { Id = 1, Email = command.Email };
            _userRepository.Setup(u => u.CreateAsync(mappedUser)).ReturnsAsync(newUser);

            var basicRole = new Role { Id = 4, Name = "BASIC_USER" };
            _rolesRepository.Setup(r => r.FindByNameAsync(DefaultRoles.BasicUser.Name))
                       .ReturnsAsync(basicRole);

            _userRoleRepo.Setup(ur => ur.CreateAsync(newUser.Id, It.IsAny<List<int>>()))
              .ReturnsAsync(1);

            _userRepository.Setup(u => u.GetByIdAsync(newUser.Id))
                .ReturnsAsync((User?) null);
            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Failed to retrieve user after creation.");

            _userRepository.Verify(u => u.CreateAsync(mappedUser), Times.Once);
            _userRoleRepo.Verify(ur =>
                ur.CreateAsync(newUser.Id, It.Is<List<int>>(l => l.Contains(basicRole.Id))),
                Times.Once);
        }

        // Case: Token generation failure
        [Fact]
        public async Task Handler_WhenTokenCantBeGenerated_ShouldThrowException()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "John",
                LastName = "Test"
            };

            _userRepository
                .Setup(u => u.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var mappedUser = new User { Email = command.Email };
            _mapper.Setup(m => m.Map<User>(command)).Returns(mappedUser);

            mappedUser.IsActive = true;
            mappedUser.IsBlocked = false;

            _passwordHasher.Setup(ph => ph.Hash(command.Password)).Returns("hashed");

            var newUser = new User { Id = 1, Email = command.Email };
            _userRepository.Setup(u => u.CreateAsync(mappedUser)).ReturnsAsync(newUser);

            var basicRole = new Role { Id = 4, Name = "BASIC_USER" };
            _rolesRepository.Setup(r => r.FindByNameAsync(DefaultRoles.BasicUser.Name))
                       .ReturnsAsync(basicRole);

            _userRoleRepo.Setup(ur => ur.CreateAsync(newUser.Id, It.IsAny<List<int>>()))
              .ReturnsAsync(1);

            _userRepository.Setup(u => u.GetByIdAsync(newUser.Id))
                .ReturnsAsync(newUser);

            _jwtService.Setup(j => j.GenerateToken(newUser)).Returns((string)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Failed to generate JWT token.");

            _userRepository.Verify(u => u.CreateAsync(mappedUser), Times.Once);
            _userRoleRepo.Verify(ur =>
                ur.CreateAsync(newUser.Id, It.Is<List<int>>(l => l.Contains(basicRole.Id))),
                Times.Once);
        }

        // Case: Refresh token generation failure
        [Fact]
        public async Task Handler_WhenRefreshTokenCantBeGenerated_ShouldThrowException()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "John",
                LastName = "Test"
            };

            _userRepository
                .Setup(u => u.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var mappedUser = new User { Email = command.Email };
            _mapper.Setup(m => m.Map<User>(command)).Returns(mappedUser);

            mappedUser.IsActive = true;
            mappedUser.IsBlocked = false;

            _passwordHasher.Setup(ph => ph.Hash(command.Password)).Returns("hashed");

            var newUser = new User { Id = 1, Email = command.Email };
            _userRepository.Setup(u => u.CreateAsync(mappedUser)).ReturnsAsync(newUser);

            var basicRole = new Role { Id = 4, Name = "BASIC_USER" };
            _rolesRepository.Setup(r => r.FindByNameAsync(DefaultRoles.BasicUser.Name))
                       .ReturnsAsync(basicRole);

            _userRoleRepo.Setup(ur => ur.CreateAsync(newUser.Id, It.IsAny<List<int>>()))
              .ReturnsAsync(1);

            _userRepository.Setup(u => u.GetByIdAsync(newUser.Id))
                .ReturnsAsync(newUser);

            _jwtService.Setup(j => j.GenerateToken(newUser)).Returns("access-token");
            _jwtService.Setup(j => j.GetJwtId("access-token")).Returns("jwt-id");

            _refreshTokenService.Setup(rt => rt.GenerateRefreshToken()).Returns((string)null);

            var handler = CreateHandler();

            // Act
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Failed to generate refresh token.");

            _userRepository.Verify(u => u.CreateAsync(mappedUser), Times.Once);
            _userRoleRepo.Verify(ur =>
                ur.CreateAsync(newUser.Id, It.Is<List<int>>(l => l.Contains(basicRole.Id))),
                Times.Once);
        }

        // Case: User registered successfuly
        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "John",
                LastName = "Test"
            };

            _userRepository
                .Setup(u => u.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            var mappedUser = new User { Email = command.Email };
            _mapper.Setup(m => m.Map<User>(command)).Returns(mappedUser);

            mappedUser.IsActive = true;
            mappedUser.IsBlocked = false;

            _passwordHasher.Setup(ph => ph.Hash(command.Password)).Returns("hashed");

            var newUser = new User { Id = 1, Email = command.Email };
            _userRepository.Setup(u => u.CreateAsync(mappedUser)).ReturnsAsync(newUser);

            var basicRole = new Role { Id = 4, Name = "BASIC_USER" };
            _rolesRepository.Setup(r => r.FindByNameAsync(DefaultRoles.BasicUser.Name))
                       .ReturnsAsync(basicRole);

            _userRoleRepo.Setup(ur => ur.CreateAsync(newUser.Id, It.IsAny<List<int>>()))
               .ReturnsAsync(1);

            _userRepository.Setup(u => u.GetByIdAsync(newUser.Id))
                   .ReturnsAsync(newUser);

            _jwtService.Setup(j => j.GenerateToken(newUser)).Returns("access-token");
            _jwtService.Setup(j => j.GetJwtId("access-token")).Returns("jwt-id");

            _refreshTokenService.Setup(rt => rt.GenerateRefreshToken()).Returns("refresh-token");
            _refreshTokenService.Setup(rt => rt.ComputeSha256("refresh-token")).Returns("refresh-token-hash");

            _refreshRepo.Setup(urt => urt.CreateAsync(newUser.Id, "jwt-id", "refresh-token-hash"))
                        .Returns(Task.FromResult<string>("refresh-token-hash"));

            var handler = CreateHandler();

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Data.Tokens.AccessToken.Should().Be("access-token");
            response.Data.Tokens.RefreshToken.Should().Be("refresh-token");
            response.Message.Should().Be("User registered successfully.");

            _userRepository.Verify(u => u.CreateAsync(mappedUser), Times.Once);
            _userRoleRepo.Verify(ur => ur.CreateAsync(newUser.Id, It.Is<List<int>>(l => l.Contains(basicRole.Id))), Times.Once);
            _refreshRepo.Verify(urt => urt.CreateAsync(newUser.Id, "jwt-id", "refresh-token-hash"), Times.Once);
        }
    }
}
