using AutoMapper;
using FluentAssertions;
using Moq;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Services;

namespace ToDo.Teste.Infrastructure.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Create_Should_Create_User_When_Email_Not_Exist()
        {
            // Arrange
            var userRequest = new UserRequest { Name = "John Doe", Email = "john@example.com", Password = "password123" };
            var user = new User(userRequest.Name, userRequest.Email, userRequest.Password);
            _userRepositoryMock.SetupSequence( r => r.GetByEmail(userRequest.Email))
                .ReturnsAsync((User)null)
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse { Name = userRequest.Name, Email = userRequest.Email });

            // Act
            var result = await _userService.Create(userRequest);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(userRequest.Name);
            result.Email.Should().Be(userRequest.Email);
            _userRepositoryMock.Verify(r => r.Create(It.Is<User>(u => u.Email == userRequest.Email)), Times.Once);
        }


        [Fact]
        public async Task Create_Should_Throw_Exception_When_Email_Already_Exists()
        {
            // Arrange
            var userRequest = new UserRequest { Name = "John Doe", Email = "john@example.com", Password = "password123" };
            var existingUser = new User("Existing User", "john@example.com", "hashedpassword");
            _userRepositoryMock.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync(existingUser);

            // Act
            Func<Task> act = async () => await _userService.Create(userRequest);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Email already belongs to another user");
        }

        [Fact]
        public async Task Create_Should_Throw_Exception_When_Failed_To_Save_User()
        {
            // Arrange
            var userRequest = new UserRequest { Name = "John Doe", Email = "john@example.com", Password = "password123" };
            _userRepositoryMock.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _userService.Create(userRequest);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Failed save user");
        }

        [Fact]
        public async Task Delete_Should_Delete_User_When_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.Delete(userId)).ReturnsAsync(true);

            // Act
            var result = await _userService.Delete(userId);

            // Assert
            result.Should().BeTrue();
            _userRepositoryMock.Verify(r => r.Delete(userId), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Return_False_When_Failed_To_Delete_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.Delete(userId)).ReturnsAsync(false);

            // Act
            var result = await _userService.Delete(userId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetById_Should_Return_User_When_User_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("John Doe", "john@example.com", "hashedpassword");
            _userRepositoryMock.Setup(r => r.GetActive(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetById_Should_Throw_Exception_When_User_Not_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetActive(userId)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _userService.GetById(userId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User with ID * not found.");
        }

        [Fact]
        public async Task Update_Should_Update_User_When_Valid_User()
        {
            // Arrange
            var user = new User("John Doe", "john@example.com", "hashedpassword");
            _userRepositoryMock.Setup(r => r.Update(user)).ReturnsAsync(true);

            // Act
            var result = await _userService.Update(user);

            // Assert
            result.Should().BeTrue();
            _userRepositoryMock.Verify(r => r.Update(user), Times.Once);
        }

        [Fact]
        public async Task GetByEmail_Should_Return_User_When_Exists()
        {
            // Arrange
            var email = "john@example.com";
            var user = new User("John Doe", email, "hashedpassword");
            _userRepositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(new UserResponse { Name = "John Doe", Email = email });

            // Act
            var result = await _userService.GetByEmail(email);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
        }

        [Fact]
        public async Task GetByEmail_Should_Throw_Exception_When_User_Not_Exist()
        {
            // Arrange
            var email = "john@example.com";
            _userRepositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _userService.GetByEmail(email);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User with email * not found.");
        }

    }
}