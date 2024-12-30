using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Teste.Infrastructure.Repository
{
    public class UserRepositoryTests
    {
        private ToDoContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ToDoContext(options);
        }

        private async Task<UserRepository> SetupRepositoryAsync(User user, ToDoContext context)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserRepository(context);
        }

        [Fact]
        public async Task Create_Should_Add_User_To_Database()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var user = new User("John Doe", "john54@example.com", "password123");
            var repository = new UserRepository(context);

            // Act
            var result = await repository.Create(user);

            // Assert
            result.Should().BeTrue();
            context.Users.Should().ContainSingle(u => u.Email == "john54@example.com"); 
        }

        [Fact]
        public async Task Get_Should_Return_User_By_Id()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var user = new User("John Doe", "john232@example.com", "password123");
            var repository = await SetupRepositoryAsync(user, context);

            // Act
            var result = await repository.Get(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("john232@example.com");
        }

        [Fact]
        public async Task GetByEmail_Should_Return_User_By_Email()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var user = new User("Jane Doe", "jane64564@example.com", "password456");
            var repository = await SetupRepositoryAsync(user, context);

            // Act
            var result = await repository.GetByEmail("jane64564@example.com");

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Jane Doe");
        }

        [Fact]
        public async Task Update_Should_Modify_User_Data()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var user = new User("John Doe", "john523@example.com", "password123");
            var repository = await SetupRepositoryAsync(user, context);

            // Act
            user.UpdateName("John Updated");
            user.UpdateEmail("john543@example.com");
            user.UpdatePassword("password124");
            var result = await repository.Update(user);

            // Assert
            result.Should().BeTrue();
            var updatedUser = await context.Users.FindAsync(user.Id);
            updatedUser.Name.Should().Be("John Updated");
            updatedUser.Email.Should().Be("john543@example.com");
            updatedUser.Password.Should().Be("password124");
        }

        [Fact]
        public async Task Delete_Should_Set_Active_To_False_When_User_Exists()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var user = new User("John Doe", "john1246@example.com", "password123");
            var repository = await SetupRepositoryAsync(user, context);

            // Act
            var result = await repository.Delete(user.Id);

            // Assert
            result.Should().BeTrue();
            var updatedUser = await repository.Get(user.Id);
            updatedUser.Active.Should().BeFalse();
        }

        [Fact]
        public async Task Delete_Should_Throw_Exception_When_User_Does_Not_Exist()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new UserRepository(context);

            // Act
            Func<Task> act = async () => await repository.Delete(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("User with ID * not found.");
        }
    }
}
