using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(Guid Id);
        Task<T> GetActive(Guid Id);
        Task<T> Get(Guid Id);
    }
}
