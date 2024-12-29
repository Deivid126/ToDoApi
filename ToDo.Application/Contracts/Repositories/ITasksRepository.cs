using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Repositories
{
    public interface ITasksRepository : IBaseRepository<Tasks>
    {
        IEnumerable<Tasks> GetAllByUser(Guid userId);
    }
}
