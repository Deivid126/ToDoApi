using FluentAssertions;
using ToDo.Domain.Entities;

namespace ToDo.Teste.Domain.Entities
{
    public sealed class UserTests
    {
        [Fact]
        public void User_Should_Initialize_With_Correct_Values()
        {
            //Arrange
            var expectName = "teste";
            var expectEmail = "teste@gmail.com";
            var expectPassword = "password";

            //Act
            var user = new User(expectName, expectEmail, expectPassword);

            //Assert
            user.Name.Should().Be(expectName);
            user.Email.Should().Be(expectEmail);
            user.Password.Should().Be(expectPassword);
            user.Active.Should().BeTrue();
            user.tasks.Should().BeEmpty();
        }

        [Fact]
        public void SetAcitve_Should_Update_Active_Status()
        {
            // Arrange
            var user = new User("John Doe", "john.doe@example.com", "password123");

            // Act
            user.UpdateActive(true);

            // Assert
            user.Active.Should().BeTrue();

            // Act again
            user.UpdateActive(false);

            // Assert
            user.Active.Should().BeFalse();
        }
    }
}
