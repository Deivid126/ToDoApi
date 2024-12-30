using FluentAssertions;
using ToDo.Domain.Entities;

namespace ToDo.Teste.Domain.Entities
{
    public class TasksTests
    {
        [Fact]
        public void Tasks_Should_Initialize_With_Correct_Values()
        {
            // Arrange
            var name = "Task 1";
            var description = "This is a test task.";
            var idUser = Guid.NewGuid();

            // Act
            var task = new Tasks(name, description, idUser);

            // Assert
            task.Name.Should().Be(name);
            task.Description.Should().Be(description);
            task.IdUser.Should().Be(idUser);
            task.Active.Should().BeTrue();
            task.User.Should().BeNull();
        }

        [Fact]
        public void UpdateActive_Should_Update_Active_Status()
        {
            // Arrange
            var task = new Tasks("Task 1", "This is a test task.", Guid.NewGuid());

            // Act
            task.UpdateActive(true);

            // Assert
            task.Active.Should().BeTrue();

            // Act again
            task.UpdateActive(false);

            // Assert
            task.Active.Should().BeFalse();
        }
    }
}
