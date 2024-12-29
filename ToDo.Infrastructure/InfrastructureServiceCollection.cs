using Microsoft.Extensions.DependencyInjection;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Repositories;
using ToDo.Infrastructure.Services;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using AutoMapper;
using System.Reflection;

namespace ToDo.Infrastructure
{
    public static class InfrastructureServiceCollection
    {
        public static void AddDependecyInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ToDoContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<IServiceUser, ServiceUser>();
            services.AddScoped<IServiceTasks, ServiceTasks>();
            services.AddScoped<IMapper, Mapper>();
        }
    }
}
