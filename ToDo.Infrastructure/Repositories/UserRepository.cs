using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure.Context;
using ToDo.Application.Contracts.Repositories;
using ToDo.Domain.Entities;
using ToDo.Application.DTOs;
using static BCrypt.Net.BCrypt;

namespace ToDo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoContext _context;

        public UserRepository(ToDoContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(User entity)
        {
            await _context.Users.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await GetActive(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
            user.UpdateActive(false);
            user.UpdateDeleteDate();
            return await Update(user);
        }

        public async Task<User> GetActive(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.Active);
        }

        public async Task<User> Get(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> Update(User entity)
        {
            _context.Users.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetByEmail(string email) 
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Active);
        }

        public async Task<bool> VerifyUserAndPassword(UserRequest userRequest)
        {
            var user = await GetByEmail(userRequest.Email) ?? throw new ArgumentException("User not exist");
            return Verify(userRequest.Password, user.Password);
        }
    }
}