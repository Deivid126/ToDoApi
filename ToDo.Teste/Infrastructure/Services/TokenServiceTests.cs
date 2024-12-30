using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Services;

namespace ToDo.Teste.Infrastructure.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ITokenService _tokenService;

        public TokenServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenService = new TokenService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GenerateJwtToken_Should_Return_Token_When_User_Exists_And_Password_Is_Correct()
        {
            // Arrange
            var userRequest = new UserRequest { Email = "john@example.com", Password = "password123" };
            var user = new User("John Doe", userRequest.Email, "hashedpassword123");  // Senha "hash" correta

            // Mock para GetByEmail
            _userRepositoryMock.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.VerifyUserAndPassword(userRequest)).ReturnsAsync(true);

            // Act
            var token = await _tokenService.GenerateJwtToken(userRequest);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GenerateJwtToken_Should_Throw_SecurityTokenValidationException_When_Password_Is_Incorrect()
        {
            // Arrange
            var userRequest = new UserRequest { Email = "john@example.com", Password = "password123" };
            var user = new User("John Doe", userRequest.Email, "hashedpassword123"); 
            _userRepositoryMock.Setup(r => r.GetByEmail(userRequest.Email)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.VerifyUserAndPassword(userRequest)).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _tokenService.GenerateJwtToken(userRequest);

            // Assert
            await act.Should().ThrowAsync<SecurityTokenValidationException>()
                .WithMessage("User or password not correct");
        }

        [Fact]
        public void VerifyUserTokenIsEqualsUserRequest_Should_Throw_SecurityTokenException_When_Token_Belongs_To_Another_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var differentUserId = Guid.NewGuid();

            var claims = new[] { new Claim(JwtRegisteredClaimNames.Jti, differentUserId.ToString()) };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            Action act = () => _tokenService.VerifyUserTokenIsEqualsUserRequest(claimsPrincipal, userId);

            // Assert
            act.Should().Throw<SecurityTokenException>()
                .WithMessage("This token belongs to another user");
        }

        [Fact]
        public void VerifyUserTokenIsEqualsUserRequest_Should_Not_Throw_Any_Exception_When_Token_Belongs_To_Correct_User()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var claims = new[] { new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()) };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            Action act = () => _tokenService.VerifyUserTokenIsEqualsUserRequest(claimsPrincipal, userId);

            // Assert
            act.Should().NotThrow();
        }
    }
}
