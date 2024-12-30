using AutoMapper;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Services;

namespace ToDo.Teste.Infrastructure.Services
{
    public class TasksServiceTests
    {
        private readonly Mock<ITasksRepository> _taskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TasksService _tasksService;

        public TasksServiceTests()
        {
            _taskRepositoryMock = new Mock<ITasksRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _tasksService = new TasksService(_taskRepositoryMock.Object, _mapperMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_Should_Create_Task_When_User_Exists()
        {
            // Arrange
            var taskRequest = new TasksRequest { IdUser = Guid.NewGuid(), Name = "Task 1", Description = "Description" };
            var task = new Tasks(taskRequest.Name, taskRequest.Description, taskRequest.IdUser);
            task.UpdateId(taskRequest.Id);
            var taskResponse = new TasksResponse { Id = taskRequest.Id, Name = taskRequest.Name, Description = taskRequest.Description, IdUser = taskRequest.IdUser };
            var user = new User("John", "john@example.com", "password123");
            _userRepositoryMock.Setup(r => r.GetActive(taskRequest.IdUser)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(r => r.Create(It.IsAny<Tasks>())).ReturnsAsync(true);
            _taskRepositoryMock.Setup(r => r.GetActive(taskRequest.Id)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TasksResponse>(task)).Returns(taskResponse);

            // Act
            var result = await _tasksService.Create(taskRequest);

            // Assert
            result.Should().NotBeNull();
            _taskRepositoryMock.Verify(r => r.Create(It.IsAny<Tasks>()), Times.Once);
        }

        [Fact]
        public async Task Create_Should_Throw_Exception_When_User_Not_Exist()
        {
            // Arrange
            var taskRequest = new TasksRequest { IdUser = Guid.NewGuid(), Name = "Task 1", Description = "Description" };
            _userRepositoryMock.Setup(r => r.GetActive(taskRequest.IdUser)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _tasksService.Create(taskRequest);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User with ID * not found.");
        }

        [Fact]
        public async Task Delete_Should_Call_Delete_On_Repository()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.Delete(taskId)).ReturnsAsync(true);

            // Act
            var result = await _tasksService.Delete(taskId);

            // Assert
            result.Should().BeTrue();
            _taskRepositoryMock.Verify(r => r.Delete(taskId), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Return_Tasks_For_Active_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var tasks = new List<Tasks> { new Tasks("Task 1", "Description", userId) };
            var tasksResponse = new List<TasksResponse> { new TasksResponse { Name = "Task 1", Description = "Description" } };

            _userRepositoryMock.Setup(r => r.GetActive(userId)).ReturnsAsync(new User("John", "john@example.com", "password123"));
            _taskRepositoryMock.Setup(r => r.GetAllByUser(userId)).Returns(tasks);
            _mapperMock.Setup(m => m.Map<IEnumerable<TasksResponse>>(tasks)).Returns(tasksResponse);

            // Act
            var result = await _tasksService.GetAll(userId);

            // Assert
            result.Should().BeEquivalentTo(tasksResponse);
        }

        [Fact]
        public async Task GetAll_Should_Throw_Exception_When_User_Not_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetActive(userId)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _tasksService.GetAll(userId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Task's with ID * not found.");
        }

        [Fact]
        public async Task GetTask_Should_Return_Task_When_Task_Exists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new Tasks("Task 1", "Description", Guid.NewGuid());
            var taskResponse = new TasksResponse { Name = "Task 1", Description = "Description" };

            _taskRepositoryMock.Setup(r => r.GetActive(taskId)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TasksResponse>(task)).Returns(taskResponse);

            // Act
            var result = await _tasksService.GetTask(taskId);

            // Assert
            result.Should().BeEquivalentTo(taskResponse);
        }

        [Fact]
        public async Task GetTask_Should_Throw_Exception_When_Task_Not_Exist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.GetActive(taskId)).ReturnsAsync((Tasks)null);

            // Act
            Func<Task> act = async () => await _tasksService.GetTask(taskId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Task with ID * not found.");
        }

        [Fact]
        public async Task Update_Should_Update_Task_When_User_Exists()
        {
            // Arrange
            var taskRequest = new TasksRequest { IdUser = Guid.NewGuid(), Name = "Updated Task", Description = "Updated Description" };
            var task = new Tasks(taskRequest.Name, taskRequest.Description, taskRequest.IdUser);
            task.UpdateId(taskRequest.Id);
            var taskResponse = new TasksResponse { Id = taskRequest.Id, Name = taskRequest.Name, Description = taskRequest.Description, IdUser = taskRequest.IdUser };
            var user = new User("John", "john@example.com", "password123"); 
            _userRepositoryMock.Setup(r => r.GetActive(taskRequest.IdUser)).ReturnsAsync(user);
            _taskRepositoryMock.SetupSequence(r => r.GetActive(taskRequest.Id))
                .ReturnsAsync(task)
                .ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TasksResponse>(task)).Returns(taskResponse);
            _taskRepositoryMock.Setup(r => r.Update(It.IsAny<Tasks>())).ReturnsAsync(true);


            // Act
            var result = await _tasksService.Update(taskRequest);

            // Assert
            result.Should().NotBeNull();
            _taskRepositoryMock.Verify(r => r.Update(It.IsAny<Tasks>()), Times.Once);  
        }

        [Fact]
        public async Task Update_Should_Throw_Exception_When_User_Not_Exist()
        {
            // Arrange
            var taskRequest = new TasksRequest { IdUser = Guid.NewGuid(), Name = "Updated Task", Description = "Updated Description" };
            _userRepositoryMock.Setup(r => r.GetActive(taskRequest.IdUser)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _tasksService.Update(taskRequest);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User with ID * not found.");
        }
    }
}
