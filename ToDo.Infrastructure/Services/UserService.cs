using AutoMapper;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;
using static BCrypt.Net.BCrypt;

namespace ToDo.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private const int WorkFactor = 12;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Create(UserRequest user)
        {
            var userExist = await _repository.GetByEmail(user.Email);
            if (userExist != null)
                throw new InvalidOperationException("Email already belongs to another user");

            user.Password = HashPassword(user.Password, WorkFactor);
            var newUser = new User(user.Name, user.Email, user.Password);
            newUser.UpdateCreateDate(null);
            if (await _repository.Create(newUser)) 
                return await GetByEmail(user.Email);
            throw new InvalidOperationException("Failed save user");
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        public async Task<User> GetById(Guid id)
        {
            return await _repository.GetActive(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public async Task<bool> Update(User user)
        {
            return await _repository.Update(user);
        }

        public async Task<UserResponse> GetByEmail(string email)
        {
            return _mapper.Map<UserResponse>(await _repository.GetByEmail(email)) ?? throw new KeyNotFoundException($"User with email {email} not found.");
        }
    }
}
