using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure.Context;
using ToDo.Application.Contracts.Repositories;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly ToDoContext _context;

        public TasksRepository(ToDoContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Tasks entity)
        {
            await _context.Tasks.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var task = await Get(id) ?? throw new KeyNotFoundException($"Task with ID {id} not found.");
            task.UpdateActive(false);
            task.UpdateDeleteDate();
            return await Update(task);
        }

        public async Task<Tasks> GetActive(Guid id)
        {
            return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Active);
        }

        public async Task<Tasks> Get(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public IEnumerable<Tasks> GetAllByUser(Guid userId)
        {
            return _context.Tasks.AsNoTracking()
                                    .Include(t => t.User)
                                    .Where(u => u.User.Id == userId && u.Active)
                                    .ToList();
        }

        public async Task<bool> Update(Tasks entity)
        {
            _context.Tasks.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}