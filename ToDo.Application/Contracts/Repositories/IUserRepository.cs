using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Repositories
{
    public interface IUserRepository : IBaseRepository<User> 
    {
        Task<User> GetByEmail(string email);
    }
}