using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserRequest user);
        Task<bool> Update(User user);
        Task<bool> Delete(Guid id);
        Task<User> GetById(Guid id);
        Task<UserResponse> GetByEmail(string email);
    }
}
