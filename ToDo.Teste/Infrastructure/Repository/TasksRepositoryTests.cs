using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Teste.Infrastructure.Repository
{
    public class TasksRepositoryTests
    {
        private ToDoContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ToDoContext(options);
        }
        private async Task<TasksRepository> SetupRepositoryAsync(Tasks task, ToDoContext context)
        {
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            return new TasksRepository(context);
        }

        [Fact]
        public async Task Create_Should_Add_Task_To_Database()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var task = new Tasks("Task 1", "Description 1", Guid.NewGuid());
            var repository = new TasksRepository(context);

            // Act
            var result = await repository.Create(task);

            // Assert
            result.Should().BeTrue();
            var createdTask = await repository.Get(task.Id);
            createdTask.Should().NotBeNull();
            createdTask.Name.Should().Be("Task 1");
        }

        [Fact]
        public async Task Delete_Should_Set_Task_Active_False_When_Task_Exists()
        {
            // Arrange
            var task = new Tasks("Task 1", "Description 1", Guid.NewGuid());
            var context = CreateInMemoryContext();
            var repository = await SetupRepositoryAsync(task, context);

            // Act
            var result = await repository.Delete(task.Id);

            // Assert
            result.Should().BeTrue();
            var deletedTask = await repository.Get(task.Id);
            deletedTask.Should().NotBeNull();
            deletedTask.Active.Should().BeFalse();
        }

        [Fact]
        public async Task Delete_Should_Throw_Exception_When_Task_Not_Found()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new TasksRepository(context);

            // Act
            Func<Task> act = async () => await repository.Delete(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Task with ID * not found.");
        }

        [Fact]
        public async Task Get_Should_Return_Task_When_Task_Exists()
        {
            // Arrange
            var task = new Tasks("Task 1", "Description 1", Guid.NewGuid());
            var context = CreateInMemoryContext();
            var repository = await SetupRepositoryAsync(task, context);

            // Act
            var result = await repository.Get(task.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Task 1");
        }

        [Fact]
        public async Task GetActive_Should_Return_Null_When_Task_Active_Is_False()
        {
            // Arrange
            var task = new Tasks("Task 1", "Description 1", Guid.NewGuid());
            var context = CreateInMemoryContext();
            var repository = await SetupRepositoryAsync(task, context);
            task.UpdateActive(false);
            await repository.Update(task);

            // Act
            var result = await repository.GetActive(task.Id);

            // Assert
            result.Should().BeNull();
        }
    }
}
