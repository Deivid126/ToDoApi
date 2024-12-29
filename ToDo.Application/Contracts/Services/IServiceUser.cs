using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Services
{
    public interface IServiceUser
    {
        Task<UserResponse> Create(UserRequest user);
        Task<bool> Update(User user);
        Task<bool> Delete(Guid id);
        Task<User> GetById(Guid id);
    }
}
