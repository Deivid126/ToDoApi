using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Services
{
    public interface ITasksService
    {
        Task<TasksResponse> Create(TasksRequest task);
        Task<TasksResponse> Update(TasksRequest task);
        Task<bool> Delete(Guid id);
        Task<TasksResponse> GetTask(Guid id);
        Task<IEnumerable<TasksResponse>> GetAll(Guid idUser);
    }
}