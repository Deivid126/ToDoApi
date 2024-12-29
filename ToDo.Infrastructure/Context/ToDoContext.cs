using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Context
{
    public class ToDoContext : DbContext
    {
        public ToDoContext() { }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
