using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Services
{
    public interface IServiceTasks
    {
        Task<bool> Create(TasksRequest task);
        Task<bool> Update(TasksRequest task);
        Task<bool> Delete(Guid id);
        Task<TasksReponse> GetTask(Guid id);
        IEnumerable<TasksReponse> GetAll(Guid idUser);
    }
}