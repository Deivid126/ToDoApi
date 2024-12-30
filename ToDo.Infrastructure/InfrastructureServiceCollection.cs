using Microsoft.Extensions.DependencyInjection;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Repositories;
using ToDo.Infrastructure.Services;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using AutoMapper;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Infrastructure
{
    public static class InfrastructureServiceCollection
    {
        public static void AddDependecyInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ToDoContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"), opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ToDoContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IMapper, Mapper>();
        }
    }
}
