using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure.Context;
using ToDo.Application.Contracts.Repositories;
using ToDo.Domain.Entities;

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
            await _context.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();
            return await _context.SaveChangesAsync() > 0;
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
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}